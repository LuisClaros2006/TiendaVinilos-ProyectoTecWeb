using System;
using System.Collections.Generic;
using System.Text;


using VinylStore.Core.Entities;

namespace VinylStore.Core.Interfaces
{
    public interface ISaleRepository
        : IBaseRepository<Sale>
    {
        Task<IEnumerable<Sale>> GetAllSalesWithDetailsAsync();

        Task<Sale?> GetSaleWithDetailsByIdAsync(int id);

        Task<Vinyl?> GetVinylForUpdateAsync(int id);

        Task<Sale?> GetSaleForUpdateAsync(int id);
    }
}
