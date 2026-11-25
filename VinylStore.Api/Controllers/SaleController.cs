
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VinylStore.Api.Response;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;
using VinylStore.Core.Exceptions;
using VinylStore.Services.Interfaces;
using VinylStore.Services.Validators;

namespace VinylStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly CrearSaleDtoValidator _crearValidator;

        public SaleController(
            ISaleService saleService,
            IMapper mapper,
            CrearSaleDtoValidator crearValidator)
        {
            _saleService = saleService;
            _mapper = mapper;
            _crearValidator = crearValidator;
        }

        #region Dto-AutoMapper

        // Consultar todas las ventas
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetSalesDtoMapper()
        {
            var sales = await _saleService.GetAllSalesAsync();

            var salesDto =
                _mapper.Map<IEnumerable<SaleDto>>(sales);

            return Ok(
                new ApiResponse<IEnumerable<SaleDto>>(salesDto));
        }

        // Consultar una venta por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetSaleByIdDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            var sale = await _saleService.GetSaleByIdAsync(id);

            if (sale == null)
            {
                throw new BusinessException(
                    "La venta no existe.",
                    HttpStatusCode.NotFound);
            }

            var saleDto = _mapper.Map<SaleDto>(sale);

            return Ok(new ApiResponse<SaleDto>(saleDto));
        }

        // Registrar una nueva venta
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertSaleDtoMapper(
            [FromBody] SaleDto saleDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(saleDto);

            if (!validationResult.IsValid)
            {
                throw new BusinessException(
                    "Error de validación.",
                    details: validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    }).ToList());
            }

            var sale = _mapper.Map<Sale>(saleDto);

            await _saleService.InsertSale(sale);

            // Recuperar la venta con sus detalles guardados
            var savedSale =
                await _saleService.GetSaleByIdAsync(sale.Id);

            var resultDto = _mapper.Map<SaleDto>(savedSale);

            return CreatedAtAction(
                nameof(GetSaleByIdDtoMapper),
                new { id = sale.Id },
                resultDto);
        }

        // Cancelar una venta
        [HttpPatch("dto/mapper/{id}/cancel")]
        public async Task<IActionResult> CancelSaleDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _saleService.CancelSale(id);

            return NoContent();
        }

        #endregion
    }
}
