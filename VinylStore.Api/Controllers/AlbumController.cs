
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
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
        private readonly CrearAlbumDtoValidator _crearValidator;
        private readonly ActualizarAlbumDtoValidator _actualizarValidator;

        public AlbumController(
            IAlbumService albumService,
            IMapper mapper,
            CrearAlbumDtoValidator crearValidator,
            ActualizarAlbumDtoValidator actualizarValidator)
        {
            _albumService = albumService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // Consultar todos los álbumes
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetAlbumsDtoMapper()
        {
            var albums = await _albumService.GetAllAlbumsAsync();

            var albumsDto = _mapper.Map<IEnumerable<AlbumDto>>(albums);

            return Ok(new ApiResponse<IEnumerable<AlbumDto>>(albumsDto));
        }

        // Consultar álbum por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetAlbumByIdDtoMapper(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);

            if (album == null)
            {
                throw new BusinessException(
                    "El álbum no existe.",
                    HttpStatusCode.NotFound);
            }

            var albumDto = _mapper.Map<AlbumDto>(album);

            return Ok(new ApiResponse<AlbumDto>(albumDto));
        }

        // Registrar álbum
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertAlbumDtoMapper(
            [FromBody] AlbumDto albumDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(albumDto);

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

            var album = _mapper.Map<Album>(albumDto);

            await _albumService.InsertAlbum(album);

            var resultDto = _mapper.Map<AlbumDto>(album);

            return CreatedAtAction(
                nameof(GetAlbumByIdDtoMapper),
                new { id = album.Id },
                resultDto);
        }

        // Actualizar álbum
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateAlbumDtoMapper(
            int id,
            [FromBody] AlbumDto albumDto)
        {
            if (id != albumDto.Id)
            {
                throw new BusinessException(
                    "El ID de la ruta no coincide con el ID del álbum.");
            }

            var validationResult =
                await _actualizarValidator.ValidateAsync(albumDto);

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

            var existingAlbum =
                await _albumService.GetAlbumByIdAsync(id);

            if (existingAlbum == null)
            {
                throw new BusinessException(
                    "El álbum no existe.",
                    HttpStatusCode.NotFound);
            }

            _mapper.Map(albumDto, existingAlbum);

            await _albumService.UpdateAlbum(existingAlbum);

            var resultDto = _mapper.Map<AlbumDto>(existingAlbum);

            return Ok(resultDto);
        }

        // Eliminar álbum
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteAlbumDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _albumService.DeleteAlbum(id);

            return NoContent();
        }

        #endregion
    }
}
