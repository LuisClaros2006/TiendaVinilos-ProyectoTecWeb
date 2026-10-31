using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;

namespace VinylStore.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task InsertEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeactivateEmployee(int id);
    }
}