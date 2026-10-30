using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;

namespace VinylStore.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();

        Task<Genre?> GetGenreByIdAsync(int id);

        Task InsertGenre(Genre genre);

        Task UpdateGenre(Genre genre);

        Task DeleteGenre(int id);
    }
}
