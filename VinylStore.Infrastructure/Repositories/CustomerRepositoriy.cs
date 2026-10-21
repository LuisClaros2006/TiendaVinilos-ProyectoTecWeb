using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;
using VinylStore.Core.Interfaces;
using VinylStore.Infrastructure.Data;

namespace VinylStore.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly VinylStoreContext _vinylStoreContext;

        public CustomerRepository(VinylStoreContext vinylStoreContext)
        {
            _vinylStoreContext = vinylStoreContext;
        }

        public async Task InsertCustomer(Customer customer)
        {
            _vinylStoreContext.Customers.Add(customer);
            await _vinylStoreContext.SaveChangesAsync();
        }
    }
}
