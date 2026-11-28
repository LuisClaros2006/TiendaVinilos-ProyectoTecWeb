using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using VinylStore.Core.Entities;
using VinylStore.Core.Exceptions;
using VinylStore.Core.Interfaces;
using VinylStore.Services.Interfaces;

namespace VinylStore.Services.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IBaseRepository<Customer> _customerRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Saledetail> _detailRepository;
        private readonly IBaseRepository<Vinyl> _vinylRepository;
        private readonly IBaseRepository<Inventorymovement> _movementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(
            ISaleRepository saleRepository,
            IBaseRepository<Customer> customerRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<Saledetail> detailRepository,
            IBaseRepository<Vinyl> vinylRepository,
            IBaseRepository<Inventorymovement> movementRepository,
            IUnitOfWork unitOfWork)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _detailRepository = detailRepository;
            _vinylRepository = vinylRepository;
            _movementRepository = movementRepository;
            _unitOfWork = unitOfWork;
        }

        // Consultar todas las ventas con sus detalles
        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllSalesWithDetailsAsync();
        }

        // Consultar una venta por ID
        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            return await _saleRepository
                .GetSaleWithDetailsByIdAsync(id);
        }

        // Registrar una nueva venta
        public async Task InsertSale(Sale sale)
        {
            if (sale == null)
            {
                throw new BusinessException(
                    "La venta es obligatoria.");
            }

            if (sale.CustomerId <= 0 || sale.EmployeeId <= 0)
            {
                throw new BusinessException(
                    "Debe seleccionar un cliente y un empleado válidos.");
            }

            var requestedDetails = sale.Saledetails?.ToList();

            if (requestedDetails == null || requestedDetails.Count == 0)
            {
                throw new BusinessException(
                    "La venta debe incluir al menos un vinilo.");
            }

            if (requestedDetails.Any(d =>
                d.VinylId <= 0 || d.Quantity <= 0))
            {
                throw new BusinessException(
                    "Cada detalle debe tener un vinilo válido " +
                    "y una cantidad mayor que cero.");
            }

            if (requestedDetails
                .GroupBy(d => d.VinylId)
                .Any(g => g.Count() > 1))
            {
                throw new BusinessException(
                    "No puede repetir el mismo vinilo en una venta.");
            }

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var customer = await _customerRepository
                    .GetById(sale.CustomerId);

                if (customer == null || customer.IsActive != true)
                {
                    throw new BusinessException(
                        "El cliente no existe o está desactivado.");
                }

                var employee = await _employeeRepository
                    .GetById(sale.EmployeeId);

                if (employee == null || employee.IsActive != true)
                {
                    throw new BusinessException(
                        "El empleado no existe o está desactivado.");
                }

                // Bloquear los vinilos en orden ascendente
                // para reducir el riesgo de interbloqueos.
                var lockedVinyls = new Dictionary<int, Vinyl>();

                foreach (int vinylId in requestedDetails
                    .Select(d => d.VinylId)
                    .OrderBy(id => id))
                {
                    var vinyl = await _saleRepository
                        .GetVinylForUpdateAsync(vinylId);

                    if (vinyl == null || vinyl.IsActive != true)
                    {
                        throw new BusinessException(
                            $"El vinilo {vinylId} no existe o está desactivado.");
                    }

                    lockedVinyls.Add(vinylId, vinyl);
                }

                decimal total = 0;

                // Validar existencias y calcular el total
                // antes de registrar cualquier cambio.
                foreach (var detail in requestedDetails)
                {
                    var vinyl = lockedVinyls[detail.VinylId];

                    if (vinyl.Stock < detail.Quantity)
                    {
                        throw new BusinessException(
                            $"Stock insuficiente para el vinilo {vinyl.Id}.");
                    }

                    if (vinyl.Price < 0)
                    {
                        throw new BusinessException(
                            $"El vinilo {vinyl.Id} tiene un precio inválido.");
                    }

                    total = checked(
                        total + vinyl.Price * detail.Quantity);
                }

                // Evitar que EF registre los detalles originales
                // con precios enviados por el cliente.
                sale.Saledetails.Clear();

                sale.Id = 0;
                sale.SaleDate = DateTime.Now;
                sale.Status = "COMPLETED";
                sale.CancelledAt = null;
                sale.Total = total;

                // Registrar encabezado para obtener Sale.Id
                await _saleRepository.Add(sale);

                foreach (var detail in requestedDetails)
                {
                    var vinyl = lockedVinyls[detail.VinylId];
                    int stockBefore = vinyl.Stock;
                    int stockAfter = stockBefore - detail.Quantity;

                    // El precio procede exclusivamente de MySQL
                    var newDetail = new Saledetail
                    {
                        SaleId = sale.Id,
                        VinylId = vinyl.Id,
                        Quantity = detail.Quantity,
                        UnitPrice = vinyl.Price
                    };

                    await _detailRepository.Add(newDetail);

                    vinyl.Stock = stockAfter;
                    await _vinylRepository.Update(vinyl);

                    var movement = new Inventorymovement
                    {
                        VinylId = vinyl.Id,
                        EmployeeId = sale.EmployeeId,
                        SaleId = sale.Id,
                        MovementType = "SALE",
                        QuantityChange = -detail.Quantity,
                        StockBefore = stockBefore,
                        StockAfter = stockAfter,
                        Reason = "Venta de vinilos",
                        MovementDate = DateTime.Now
                    };

                    await _movementRepository.Add(movement);
                }
            });
        }

        // Cancelar una venta y restituir existencias
        public async Task CancelSale(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var sale = await _saleRepository.GetSaleForUpdateAsync(id);

                if (sale == null)
                {
                    throw new BusinessException(
                        "La venta no existe.",
                        HttpStatusCode.NotFound);
                }

                if (sale.Status != "COMPLETED")
                {
                    throw new BusinessException(
                        "La venta no está completada o ya fue cancelada.");
                }

                if (sale.Saledetails.Count == 0)
                {
                    throw new BusinessException(
                        "No se puede cancelar una venta sin detalles.");
                }

                // Bloquear vinilos en un orden consistente.
                var lockedVinyls = new Dictionary<int, Vinyl>();

                foreach (int vinylId in sale.Saledetails
                    .Select(d => d.VinylId)
                    .Distinct()
                    .OrderBy(id => id))
                {
                    var vinyl = await _saleRepository
                        .GetVinylForUpdateAsync(vinylId);

                    if (vinyl == null)
                    {
                        throw new BusinessException(
                            $"No existe el vinilo {vinylId}.");
                    }

                    lockedVinyls.Add(vinylId, vinyl);
                }

                foreach (var detail in sale.Saledetails)
                {
                    var vinyl = lockedVinyls[detail.VinylId];

                    int stockBefore = vinyl.Stock;
                    int stockAfter = checked(
                        stockBefore + detail.Quantity);

                    vinyl.Stock = stockAfter;
                    await _vinylRepository.Update(vinyl);

                    var movement = new Inventorymovement
                    {
                        VinylId = vinyl.Id,
                        EmployeeId = sale.EmployeeId,
                        SaleId = sale.Id,
                        MovementType = "SALE_CANCELLATION",
                        QuantityChange = detail.Quantity,
                        StockBefore = stockBefore,
                        StockAfter = stockAfter,
                        Reason = "Cancelación de venta",
                        MovementDate = DateTime.Now
                    };

                    await _movementRepository.Add(movement);
                }

                sale.Status = "CANCELLED";
                sale.CancelledAt = DateTime.Now;

                await _saleRepository.Update(sale);
            });
        }
    }
}
