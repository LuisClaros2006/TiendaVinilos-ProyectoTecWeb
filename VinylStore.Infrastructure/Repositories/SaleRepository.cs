using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.EntityFrameworkCore;
using VinylStore.Core.Entities;
using VinylStore.Core.Interfaces;
using VinylStore.Infrastructure.Data;

namespace VinylStore.Infrastructure.Repositories
{
    public class SaleRepository
        : BaseRepository<Sale>, ISaleRepository
    {
        private readonly VinylStoreContext _context;

        public SaleRepository(VinylStoreContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>>GetAllSalesWithDetailsAsync()
        {
            return await _context.Sales
                .AsNoTracking()
                .Include(s => s.Saledetails)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Sale?>GetSaleWithDetailsByIdAsync(int id)
        {
            return await _context.Sales
                .Include(s => s.Saledetails)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Vinyl?>GetVinylForUpdateAsync(int id)
        {
            if (_context.Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException(
                    "Esta operacion requiere una transaccion activa.");
            }

            var vinyls = await _context.Vinyls
                .FromSqlInterpolated(
                    $"SELECT * FROM `vinyl` WHERE `id` = {id} FOR UPDATE")
                .AsTracking()
                .ToListAsync();

            return vinyls.FirstOrDefault();
        }


        public async Task<Sale?> GetSaleForUpdateAsync(int id)
        {
            if (_context.Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException(
                    "Esta operación requiere una transacción activa.");
            }

            // Bloquear primero la fila de Sale
            var sale = await _context.Sales
                .FromSqlInterpolated(
                    $"SELECT * FROM `sale` WHERE `id` = {id} FOR UPDATE")
                .AsTracking()
                .SingleOrDefaultAsync();

            if (sale == null)
            {
                return null;
            }

            // Cargar los detalles después de bloquear la venta
            await _context.Entry(sale)
                .Collection(s => s.Saledetails)
                .LoadAsync();

            return sale;
        }

    }
}

