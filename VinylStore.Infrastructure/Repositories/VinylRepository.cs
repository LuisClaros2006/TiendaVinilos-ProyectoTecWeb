using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;
using VinylStore.Core.Interfaces;
using VinylStore.Infrastructure.Data;

namespace VinylStore.Infrastructure.Repositories
{
    public class VinylRepository : IVinylRepository
    {
        private readonly VinylStoreContext _vinylStoreContext;

        public VinylRepository(
            VinylStoreContext vinylStoreContext)
        {
            _vinylStoreContext = vinylStoreContext;
        }

        public async Task InsertVinyl(Vinyl vinyl)
        {
            _vinylStoreContext.Vinyls.Add(vinyl);
            await _vinylStoreContext.SaveChangesAsync();
        }
    }
}