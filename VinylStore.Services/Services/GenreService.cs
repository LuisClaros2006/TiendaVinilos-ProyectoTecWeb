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
    public class GenreService : IGenreService
    {
        private readonly IBaseRepository<Genre> _genreRepository;
        private readonly IBaseRepository<Vinyl> _vinylRepository;

        public GenreService(
            IBaseRepository<Genre> genreRepository,
            IBaseRepository<Vinyl> vinylRepository)
        {
            _genreRepository = genreRepository;
            _vinylRepository = vinylRepository;
        }

        // Consultar todos los géneros
        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAll();
        }

        // Consultar género por ID
        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _genreRepository.GetById(id);
        }

        // Registrar género
        public async Task InsertGenre(Genre genre)
        {
            ValidateGenre(genre);

            var genres = await _genreRepository.GetAll();

            bool exists = genres.Any(g =>
                g.Name.ToLower() == genre.Name.Trim().ToLower());

            if (exists)
            {
                throw new BusinessException(
                    "Ya existe un género con ese nombre.");
            }

            genre.Name = genre.Name.Trim();

            await _genreRepository.Add(genre);
        }

        // Actualizar género
        public async Task UpdateGenre(Genre genre)
        {
            var existingGenre = await _genreRepository.GetById(genre.Id);

            if (existingGenre == null)
            {
                throw new BusinessException(
                    "El género no existe.",
                    HttpStatusCode.NotFound);
            }

            ValidateGenre(genre);

            var genres = await _genreRepository.GetAll();

            bool exists = genres.Any(g =>
                g.Id != genre.Id &&
                g.Name.ToLower() == genre.Name.Trim().ToLower());

            if (exists)
            {
                throw new BusinessException(
                    "Ya existe otro género con ese nombre.");
            }

            existingGenre.Name = genre.Name.Trim();

            await _genreRepository.Update(existingGenre);
        }

        // Eliminar género
        public async Task DeleteGenre(int id)
        {
            var genre = await _genreRepository.GetById(id);

            if (genre == null)
            {
                throw new BusinessException(
                    "El género no existe.",
                    HttpStatusCode.NotFound);
            }

            var vinyls = await _vinylRepository.GetAll();

            bool isUsed = vinyls.Any(v => v.GenreId == id);

            if (isUsed)
            {
                throw new BusinessException(
                    "No se puede eliminar el género porque está asociado a uno o más vinilos.");
            }

            await _genreRepository.Delete(id);
        }

        // Validaciones de negocio
        private static void ValidateGenre(Genre genre)
        {
            if (string.IsNullOrWhiteSpace(genre.Name))
            {
                throw new BusinessException(
                    "El nombre del género es obligatorio.");
            }

            if (genre.Name.Trim().Length > 100)
            {
                throw new BusinessException(
                    "El nombre del género no puede superar 100 caracteres.");
            }
        }
    }
}