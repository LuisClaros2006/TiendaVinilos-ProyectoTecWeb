using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;
using VinylStore.Core.Interfaces;
using VinylStore.Services.Interfaces;

using VinylStore.Core.Exceptions;
using System.Net;

namespace VinylStore.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IBaseRepository<Customer> _customerRepository;

        public CustomerService(IBaseRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Consultar todos los clientes
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAll();
        }

        // Consultar cliente por ID
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetById(id);
        }

        // Registrar cliente
        public async Task InsertCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                throw new BusinessException("El nombre del cliente es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(customer.LastName))
            {
                throw new BusinessException("El apellido del cliente es obligatorio.");
            }

            customer.FirstName = customer.FirstName.Trim();
            customer.LastName = customer.LastName.Trim();

            customer.IsActive = true;
            customer.CreatedAt = DateTime.Now;

            await _customerRepository.Add(customer);
        }

        // Actualizar cliente
        public async Task UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _customerRepository.GetById(customer.Id);

            if (existingCustomer == null)
            {
                throw new BusinessException("El cliente no existe.",
                    HttpStatusCode.NotFound);
            }

            if (existingCustomer.IsActive != true)
            {
                throw new BusinessException(
                    "No se puede actualizar un cliente desactivado.");
            }

            if (string.IsNullOrWhiteSpace(customer.FirstName) ||
                string.IsNullOrWhiteSpace(customer.LastName))
            {
                throw new BusinessException("El nombre y apellido son obligatorios.");
            }

            existingCustomer.FirstName = customer.FirstName.Trim();
            existingCustomer.LastName = customer.LastName.Trim();
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;

            await _customerRepository.Update(existingCustomer);
        }

        // Desactivar cliente sin eliminarlo de la base de datos
        public async Task DeactivateCustomer(int id)
        {
            var customer = await _customerRepository.GetById(id);

            if (customer == null)
            {
                throw new BusinessException("El cliente no existe.");
            }

            if (customer.IsActive != true)
            {
                throw new BusinessException(
                    "El cliente ya está desactivado.");
            }

            customer.IsActive = false;

            await _customerRepository.Update(customer);
        }
    }
}
