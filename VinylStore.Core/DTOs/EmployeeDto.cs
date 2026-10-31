using System;
using System.Collections.Generic;
using System.Text;

namespace VinylStore.Core.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "EMPLOYEE";
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}