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
    public class EmployeeService : IEmployeeService
    {
        private readonly IBaseRepository<Employee> _employeeRepository;

        public EmployeeService(
            IBaseRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Consultar todos los empleados
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAll();
        }

        // Consultar empleado por ID
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetById(id);
        }

        // Registrar empleado
        public async Task InsertEmployee(Employee employee)
        {
            ValidateEmployee(employee);

            await ValidateUniqueEmail(employee.Email);

            employee.FirstName = employee.FirstName.Trim();
            employee.LastName = employee.LastName.Trim();
            employee.Email = employee.Email.Trim().ToLowerInvariant();
            employee.Role = employee.Role.Trim().ToUpperInvariant();

            employee.IsActive = true;
            employee.CreatedAt = DateTime.Now;
            employee.PasswordHash = null;

            await _employeeRepository.Add(employee);
        }

        // Actualizar empleado
        public async Task UpdateEmployee(Employee employee)
        {
            var existingEmployee =
                await _employeeRepository.GetById(employee.Id);

            if (existingEmployee == null)
            {
                throw new BusinessException(
                    "El empleado no existe.",
                    HttpStatusCode.NotFound);
            }

            if (existingEmployee.IsActive != true)
            {
                throw new BusinessException(
                    "No se puede actualizar un empleado desactivado.");
            }

            ValidateEmployee(employee);

            await ValidateUniqueEmail(
                employee.Email, employee.Id);

            existingEmployee.FirstName =
                employee.FirstName.Trim();

            existingEmployee.LastName =
                employee.LastName.Trim();

            existingEmployee.Email =
                employee.Email.Trim().ToLowerInvariant();

            existingEmployee.Role =
                employee.Role.Trim().ToUpperInvariant();

            // No modificamos Id, CreatedAt, IsActive
            // ni PasswordHash.

            await _employeeRepository.Update(existingEmployee);
        }

        // Desactivar empleado
        public async Task DeactivateEmployee(int id)
        {
            var employee = await _employeeRepository.GetById(id);

            if (employee == null)
            {
                throw new BusinessException(
                    "El empleado no existe.",
                    HttpStatusCode.NotFound);
            }

            if (employee.IsActive != true)
            {
                throw new BusinessException(
                    "El empleado ya está desactivado.");
            }

            employee.IsActive = false;

            await _employeeRepository.Update(employee);
        }

        // Validaciones de negocio
        private static void ValidateEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FirstName))
            {
                throw new BusinessException(
                    "El nombre del empleado es obligatorio.");
            }

            if (employee.FirstName.Trim().Length > 80)
            {
                throw new BusinessException(
                    "El nombre no puede superar 80 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(employee.LastName))
            {
                throw new BusinessException(
                    "El apellido del empleado es obligatorio.");
            }

            if (employee.LastName.Trim().Length > 80)
            {
                throw new BusinessException(
                    "El apellido no puede superar 80 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(employee.Email))
            {
                throw new BusinessException(
                    "El correo electrónico es obligatorio.");
            }

            if (employee.Email.Trim().Length > 150)
            {
                throw new BusinessException(
                    "El correo no puede superar 150 caracteres.");
            }

            string[] validRoles = { "ADMIN", "EMPLOYEE" };

            if (string.IsNullOrWhiteSpace(employee.Role) ||
                !validRoles.Contains(
                    employee.Role.Trim().ToUpperInvariant()))
            {
                throw new BusinessException(
                    "El rol debe ser ADMIN o EMPLOYEE.");
            }
        }

        // Comprobar que el correo no esté duplicado
        private async Task ValidateUniqueEmail(
            string email, int? excludeId = null)
        {
            var employees = await _employeeRepository.GetAll();

            bool exists = employees.Any(e =>
                string.Equals(
                    e.Email,
                    email.Trim(),
                    StringComparison.OrdinalIgnoreCase)
                && (!excludeId.HasValue ||
                    e.Id != excludeId.Value));

            if (exists)
            {
                throw new BusinessException(
                    "Ya existe un empleado con ese correo electrónico.");
            }
        }
    }
}