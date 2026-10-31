using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;

namespace VinylStore.Services.Interfaces
{
    public interface IVinylService
    {
        Task<IEnumerable<Vinyl>> GetAllVinylsAsync();
        Task<Vinyl?> GetVinylByIdAsync(int id);
        Task InsertVinyl(Vinyl vinyl);
        Task UpdateVinyl(Vinyl vinyl);
        Task DeactivateVinyl(int id);
    }
}
