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
    public class ArtistService : IArtistService
    {
        private readonly IBaseRepository<Artist> _artistRepository;
        private readonly IBaseRepository<Album> _albumRepository;

        public ArtistService(
            IBaseRepository<Artist> artistRepository,
            IBaseRepository<Album> albumRepository)
        {
            _artistRepository = artistRepository;
            _albumRepository = albumRepository;
        }

        // Consultar todos los artistas
        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _artistRepository.GetAll();
        }

        // Consultar artista por ID
        public async Task<Artist?> GetArtistByIdAsync(int id)
        {
            return await _artistRepository.GetById(id);
        }

        // Registrar artista
        public async Task InsertArtist(Artist artist)
        {
            ValidateArtist(artist);

            var artists = await _artistRepository.GetAll();

            bool exists = artists.Any(a =>
                a.Name.ToLower() == artist.Name.Trim().ToLower());

            if (exists)
            {
                throw new BusinessException(
                    "Ya existe un artista con ese nombre.");
            }

            artist.Name = artist.Name.Trim();

            await _artistRepository.Add(artist);
        }

        // Actualizar artista
        public async Task UpdateArtist(Artist artist)
        {
            var existingArtist = await _artistRepository.GetById(artist.Id);

            if (existingArtist == null)
            {
                throw new BusinessException(
                    "El artista no existe.",
                    HttpStatusCode.NotFound);
            }

            ValidateArtist(artist);

            var artists = await _artistRepository.GetAll();

            bool exists = artists.Any(a =>
                a.Id != artist.Id &&
                a.Name.ToLower() == artist.Name.Trim().ToLower());

            if (exists)
            {
                throw new BusinessException(
                    "Ya existe otro artista con ese nombre.");
            }

            existingArtist.Name = artist.Name.Trim();

            await _artistRepository.Update(existingArtist);
        }

        // Eliminar artista
        public async Task DeleteArtist(int id)
        {
            var artist = await _artistRepository.GetById(id);

            if (artist == null)
            {
                throw new BusinessException(
                    "El artista no existe.",
                    HttpStatusCode.NotFound);
            }

            var albums = await _albumRepository.GetAll();

            bool hasAlbums = albums.Any(a => a.ArtistId == id);

            if (hasAlbums)
            {
                throw new BusinessException(
                    "No se puede eliminar el artista porque tiene álbumes asociados.");
            }

            await _artistRepository.Delete(id);
        }

        // Validaciones de negocio
        private static void ValidateArtist(Artist artist)
        {
            if (string.IsNullOrWhiteSpace(artist.Name))
            {
                throw new BusinessException(
                    "El nombre del artista es obligatorio.");
            }

            if (artist.Name.Trim().Length > 150)
            {
                throw new BusinessException(
                    "El nombre del artista no puede superar 150 caracteres.");
            }
        }
    }
}
