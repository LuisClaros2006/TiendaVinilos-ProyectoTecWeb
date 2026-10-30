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
    public class AlbumService : IAlbumService
    {
        private readonly IBaseRepository<Album> _albumRepository;
        private readonly IBaseRepository<Artist> _artistRepository;
        private readonly IBaseRepository<Vinyl> _vinylRepository;

        public AlbumService(
            IBaseRepository<Album> albumRepository,
            IBaseRepository<Artist> artistRepository,
            IBaseRepository<Vinyl> vinylRepository)
        {
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            _vinylRepository = vinylRepository;
        }

        // Consultar todos los álbumes
        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _albumRepository.GetAll();
        }

        // Consultar álbum por ID
        public async Task<Album?> GetAlbumByIdAsync(int id)
        {
            return await _albumRepository.GetById(id);
        }

        // Registrar álbum
        public async Task InsertAlbum(Album album)
        {
            ValidateAlbum(album);

            await ValidateArtistExists(album.ArtistId);

            album.Title = album.Title.Trim();

            await _albumRepository.Add(album);
        }

        // Actualizar álbum
        public async Task UpdateAlbum(Album album)
        {
            var existingAlbum = await _albumRepository.GetById(album.Id);

            if (existingAlbum == null)
            {
                throw new BusinessException(
                    "El álbum no existe.",
                    HttpStatusCode.NotFound);
            }

            ValidateAlbum(album);

            await ValidateArtistExists(album.ArtistId);

            existingAlbum.Title = album.Title.Trim();
            existingAlbum.ArtistId = album.ArtistId;
            existingAlbum.ReleaseYear = album.ReleaseYear;

            await _albumRepository.Update(existingAlbum);
        }

        // Eliminar álbum
        public async Task DeleteAlbum(int id)
        {
            var album = await _albumRepository.GetById(id);

            if (album == null)
            {
                throw new BusinessException(
                    "El álbum no existe.",
                    HttpStatusCode.NotFound);
            }

            var vinyls = await _vinylRepository.GetAll();

            bool hasVinyls = vinyls.Any(v => v.AlbumId == id);

            if (hasVinyls)
            {
                throw new BusinessException(
                    "No se puede eliminar el álbum porque tiene vinilos asociados.");
            }

            await _albumRepository.Delete(id);
        }

        // Comprobar la existencia del artista
        private async Task ValidateArtistExists(int artistId)
        {
            if (artistId <= 0)
            {
                throw new BusinessException(
                    "Debe seleccionar un artista válido.");
            }

            var artist = await _artistRepository.GetById(artistId);

            if (artist == null)
            {
                throw new BusinessException(
                    "El artista seleccionado no existe.",
                    HttpStatusCode.NotFound);
            }
        }

        // Validaciones del álbum
        private static void ValidateAlbum(Album album)
        {
            if (string.IsNullOrWhiteSpace(album.Title))
            {
                throw new BusinessException(
                    "El título del álbum es obligatorio.");
            }

            if (album.Title.Trim().Length > 200)
            {
                throw new BusinessException(
                    "El título del álbum no puede superar 200 caracteres.");
            }

            if (album.ReleaseYear.HasValue)
            {
                int year = Convert.ToInt32(album.ReleaseYear.Value);

                if (year < 1900 || year > 2100)
                {
                    throw new BusinessException(
                        "El año de lanzamiento debe estar entre 1900 y 2100.");
                }
            }
        }
    }
}