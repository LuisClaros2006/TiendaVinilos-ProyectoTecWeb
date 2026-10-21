using System;
using System.Collections.Generic;
using System.Text;
using VinylStore.Core.Entities;

namespace VinylStore.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task InsertCustomer(Customer customer);
    }
}
