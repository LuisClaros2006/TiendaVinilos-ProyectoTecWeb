using System;
using System.Collections.Generic;
using System.Text;


namespace VinylStore.Core.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; } = "COMPLETED";

        public DateTime? CancelledAt { get; set; }

        public List<SaledetailDto> Saledetails { get; set; } = new List<SaledetailDto>();
    }
}

