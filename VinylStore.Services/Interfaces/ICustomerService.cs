using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;

namespace VinylStore.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer?> GetCustomerByIdAsync(int id);

        Task InsertCustomer(Customer customer);

        Task UpdateCustomer(Customer customer);

        Task DeactivateCustomer(int id);
    }
}