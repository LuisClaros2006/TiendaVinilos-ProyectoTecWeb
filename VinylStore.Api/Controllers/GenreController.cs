
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
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;
        private readonly CrearGenreDtoValidator _crearValidator;
        private readonly ActualizarGenreDtoValidator _actualizarValidator;

        public GenreController(
            IGenreService genreService,
            IMapper mapper,
            CrearGenreDtoValidator crearValidator,
            ActualizarGenreDtoValidator actualizarValidator)
        {
            _genreService = genreService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // Consultar todos los géneros
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetGenresDtoMapper()
        {
            var genres = await _genreService.GetAllGenresAsync();

            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);

            return Ok(new ApiResponse<IEnumerable<GenreDto>>(genresDto));
        }

        // Consultar género por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetGenreByIdDtoMapper(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);

            if (genre == null)
            {
                throw new BusinessException(
                    "El género no existe.",
                    HttpStatusCode.NotFound);
            }

            var genreDto = _mapper.Map<GenreDto>(genre);

            return Ok(new ApiResponse<GenreDto>(genreDto));
        }

        // Registrar género
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertGenreDtoMapper(
            [FromBody] GenreDto genreDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(genreDto);

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

            var genre = _mapper.Map<Genre>(genreDto);

            await _genreService.InsertGenre(genre);

            var resultDto = _mapper.Map<GenreDto>(genre);

            return CreatedAtAction(
                nameof(GetGenreByIdDtoMapper),
                new { id = genre.Id },
                resultDto);
        }

        // Actualizar género
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateGenreDtoMapper(
            int id,
            [FromBody] GenreDto genreDto)
        {
            if (id != genreDto.Id)
            {
                throw new BusinessException(
                    "El ID de la ruta no coincide con el ID del género.");
            }

            var validationResult =
                await _actualizarValidator.ValidateAsync(genreDto);

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

            var existingGenre =
                await _genreService.GetGenreByIdAsync(id);

            if (existingGenre == null)
            {
                throw new BusinessException(
                    "El género no existe.",
                    HttpStatusCode.NotFound);
            }

            _mapper.Map(genreDto, existingGenre);

            await _genreService.UpdateGenre(existingGenre);

            var resultDto = _mapper.Map<GenreDto>(existingGenre);

            return Ok(resultDto);
        }

        // Eliminar género
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteGenreDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _genreService.DeleteGenre(id);

            return NoContent();
        }

        #endregion
    }
}
