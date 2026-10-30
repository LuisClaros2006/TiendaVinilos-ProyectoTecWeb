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
    public class VinylController : ControllerBase
    {
        private readonly IVinylService _vinylService;
        private readonly IMapper _mapper;
        private readonly CrearVinylDtoValidator _crearValidator;
        private readonly ActualizarVinylDtoValidator _actualizarValidator;

        public VinylController(
            IVinylService vinylService,
            IMapper mapper,
            CrearVinylDtoValidator crearValidator,
            ActualizarVinylDtoValidator actualizarValidator)
        {
            _vinylService = vinylService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // Consultar todos los vinilos
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetVinylsDtoMapper()
        {
            var vinyls = await _vinylService.GetAllVinylsAsync();

            var vinylsDto = _mapper.Map<IEnumerable<VinylDto>>(vinyls);

            return Ok(
                new ApiResponse<IEnumerable<VinylDto>>(vinylsDto));
        }

        // Consultar un vinilo por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetVinylByIdDtoMapper(int id)
        {
            var vinyl = await _vinylService.GetVinylByIdAsync(id);

            if (vinyl == null)
            {
                throw new BusinessException(
                    "El vinilo no existe.",
                    HttpStatusCode.NotFound);
            }

            var vinylDto = _mapper.Map<VinylDto>(vinyl);

            return Ok(new ApiResponse<VinylDto>(vinylDto));
        }

        // Registrar vinilo
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertVinylDtoMapper(
            [FromBody] VinylDto vinylDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(vinylDto);

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

            var vinyl = _mapper.Map<Vinyl>(vinylDto);

            await _vinylService.InsertVinyl(vinyl);

            var resultDto = _mapper.Map<VinylDto>(vinyl);

            return CreatedAtAction(
                nameof(GetVinylByIdDtoMapper),
                new { id = vinyl.Id },
                resultDto);
        }

        // Actualizar vinilo
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateVinylDtoMapper(
            int id,
            [FromBody] VinylDto vinylDto)
        {
            if (id != vinylDto.Id)
            {
                throw new BusinessException(
                    "El ID de la ruta no coincide con el ID del vinilo.");
            }

            var validationResult =
                await _actualizarValidator.ValidateAsync(vinylDto);

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

            var existingVinyl =
                await _vinylService.GetVinylByIdAsync(id);

            if (existingVinyl == null)
            {
                throw new BusinessException(
                    "El vinilo no existe.",
                    HttpStatusCode.NotFound);
            }

            _mapper.Map(vinylDto, existingVinyl);

            await _vinylService.UpdateVinyl(existingVinyl);

            var resultDto = _mapper.Map<VinylDto>(existingVinyl);

            return Ok(resultDto);
        }

        // Desactivar vinilo
        [HttpPatch("dto/mapper/{id}/deactivate")]
        public async Task<IActionResult> DeactivateVinylDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _vinylService.DeactivateVinyl(id);

            return NoContent();
        }

        #endregion
    }
}
