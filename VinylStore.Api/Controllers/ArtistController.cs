
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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;
        private readonly CrearArtistDtoValidator _crearValidator;
        private readonly ActualizarArtistDtoValidator _actualizarValidator;

        public ArtistController(
            IArtistService artistService,
            IMapper mapper,
            CrearArtistDtoValidator crearValidator,
            ActualizarArtistDtoValidator actualizarValidator)
        {
            _artistService = artistService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // Consultar todos los artistas
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetArtistsDtoMapper()
        {
            var artists = await _artistService.GetAllArtistsAsync();

            var artistsDto = _mapper.Map<IEnumerable<ArtistDto>>(artists);

            return Ok(new ApiResponse<IEnumerable<ArtistDto>>(artistsDto));
        }

        // Consultar artista por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetArtistByIdDtoMapper(int id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);

            if (artist == null)
            {
                throw new BusinessException(
                    "El artista no existe.",
                    HttpStatusCode.NotFound);
            }

            var artistDto = _mapper.Map<ArtistDto>(artist);

            return Ok(new ApiResponse<ArtistDto>(artistDto));
        }

        // Registrar artista
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertArtistDtoMapper(
            [FromBody] ArtistDto artistDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(artistDto);

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

            var artist = _mapper.Map<Artist>(artistDto);

            await _artistService.InsertArtist(artist);

            var resultDto = _mapper.Map<ArtistDto>(artist);

            return CreatedAtAction(
                nameof(GetArtistByIdDtoMapper),
                new { id = artist.Id },
                resultDto);
        }

        // Actualizar artista
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateArtistDtoMapper(
            int id,
            [FromBody] ArtistDto artistDto)
        {
            if (id != artistDto.Id)
            {
                throw new BusinessException(
                    "El ID de la ruta no coincide con el ID del artista.");
            }

            var validationResult =
                await _actualizarValidator.ValidateAsync(artistDto);

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

            var existingArtist =
                await _artistService.GetArtistByIdAsync(id);

            if (existingArtist == null)
            {
                throw new BusinessException(
                    "El artista no existe.",
                    HttpStatusCode.NotFound);
            }

            _mapper.Map(artistDto, existingArtist);

            await _artistService.UpdateArtist(existingArtist);

            var resultDto = _mapper.Map<ArtistDto>(existingArtist);

            return Ok(resultDto);
        }

        // Eliminar artista
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteArtistDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _artistService.DeleteArtist(id);

            return NoContent();
        }

        #endregion
    }
}
