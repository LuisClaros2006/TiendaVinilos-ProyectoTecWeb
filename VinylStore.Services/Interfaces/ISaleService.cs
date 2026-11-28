using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;

namespace VinylStore.Services.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();

        Task<Sale?> GetSaleByIdAsync(int id);

        Task InsertSale(Sale sale);

        Task CancelSale(int id);
    }
}

