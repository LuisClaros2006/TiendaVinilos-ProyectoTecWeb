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
    public class VinylService : IVinylService
    {
        private readonly IBaseRepository<Vinyl> _vinylRepository;
        private readonly IBaseRepository<Album> _albumRepository;
        private readonly IBaseRepository<Genre> _genreRepository;

        public VinylService(
            IBaseRepository<Vinyl> vinylRepository,
            IBaseRepository<Album> albumRepository,
            IBaseRepository<Genre> genreRepository)
        {
            _vinylRepository = vinylRepository;
            _albumRepository = albumRepository;
            _genreRepository = genreRepository;
        }

        // Consultar todos los vinilos
        public async Task<IEnumerable<Vinyl>> GetAllVinylsAsync()
        {
            return await _vinylRepository.GetAll();
        }

        // Consultar vinilo por ID
        public async Task<Vinyl?> GetVinylByIdAsync(int id)
        {
            return await _vinylRepository.GetById(id);
        }

        // Registrar vinilo
        public async Task InsertVinyl(Vinyl vinyl)
        {
            ValidateVinyl(vinyl);

            await ValidateRelations(vinyl.AlbumId, vinyl.GenreId);

            await ValidateUniqueSku(vinyl.Sku);

            vinyl.Sku = vinyl.Sku.Trim().ToUpperInvariant();
            vinyl.Format = vinyl.Format.Trim().ToUpperInvariant();
            vinyl.IsActive = true;
            vinyl.CreatedAt = DateTime.Now;

            await _vinylRepository.Add(vinyl);
        }

        // Actualizar vinilo
        public async Task UpdateVinyl(Vinyl vinyl)
        {
            var existingVinyl = await _vinylRepository.GetById(vinyl.Id);

            if (existingVinyl == null)
            {
                throw new BusinessException(
                    "El vinilo no existe.",
                    HttpStatusCode.NotFound);
            }

            if (existingVinyl.IsActive != true)
            {
                throw new BusinessException(
                    "No se puede actualizar un vinilo desactivado.");
            }

            ValidateVinyl(vinyl);

            await ValidateRelations(vinyl.AlbumId, vinyl.GenreId);

            await ValidateUniqueSku(vinyl.Sku, vinyl.Id);

            existingVinyl.AlbumId = vinyl.AlbumId;
            existingVinyl.GenreId = vinyl.GenreId;
            existingVinyl.Sku = vinyl.Sku.Trim().ToUpperInvariant();
            existingVinyl.Format = vinyl.Format.Trim().ToUpperInvariant();
            existingVinyl.Edition = vinyl.Edition;
            existingVinyl.Description = vinyl.Description;
            existingVinyl.CoverImageUrl = vinyl.CoverImageUrl;
            existingVinyl.Price = vinyl.Price;

            await _vinylRepository.Update(existingVinyl);
        }

        // Desactivar vinilo
        public async Task DeactivateVinyl(int id)
        {
            var vinyl = await _vinylRepository.GetById(id);

            if (vinyl == null)
            {
                throw new BusinessException(
                    "El vinilo no existe.",
                    HttpStatusCode.NotFound);
            }

            if (vinyl.IsActive != true)
            {
                throw new BusinessException(
                    "El vinilo ya está desactivado.");
            }

            vinyl.IsActive = false;

            await _vinylRepository.Update(vinyl);
        }

        // Validar información del vinilo
        private static void ValidateVinyl(Vinyl vinyl)
        {
            if (string.IsNullOrWhiteSpace(vinyl.Sku))
            {
                throw new BusinessException(
                    "El SKU del vinilo es obligatorio.");
            }

            if (vinyl.Sku.Trim().Length > 40)
            {
                throw new BusinessException(
                    "El SKU no puede superar 40 caracteres.");
            }

            string[] validFormats = { "LP", "EP", "SINGLE" };

            if (string.IsNullOrWhiteSpace(vinyl.Format) ||
                !validFormats.Contains(vinyl.Format.ToUpperInvariant()))
            {
                throw new BusinessException(
                    "El formato debe ser LP, EP o SINGLE.");
            }

            if (vinyl.Price < 0)
            {
                throw new BusinessException(
                    "El precio no puede ser negativo.");
            }

            if (vinyl.Stock < 0)
            {
                throw new BusinessException(
                    "El stock no puede ser negativo.");
            }
        }

        // Verificar álbum y género
        private async Task ValidateRelations(int albumId, int genreId)
        {
            if (albumId <= 0 || genreId <= 0)
            {
                throw new BusinessException(
                    "Debe seleccionar un álbum y un género válidos.");
            }

            var album = await _albumRepository.GetById(albumId);

            if (album == null)
            {
                throw new BusinessException(
                    "El álbum seleccionado no existe.",
                    HttpStatusCode.NotFound);
            }

            var genre = await _genreRepository.GetById(genreId);

            if (genre == null)
            {
                throw new BusinessException(
                    "El género seleccionado no existe.",
                    HttpStatusCode.NotFound);
            }
        }

        // Verificar que el SKU no esté repetido
        private async Task ValidateUniqueSku(
            string sku, int? excludeId = null)
        {
            var vinyls = await _vinylRepository.GetAll();

            bool exists = vinyls.Any(v =>
                v.Sku.Equals(sku.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!excludeId.HasValue || v.Id != excludeId.Value));

            if (exists)
            {
                throw new BusinessException(
                    "Ya existe un vinilo con ese SKU.");
            }
        }
    }
}
