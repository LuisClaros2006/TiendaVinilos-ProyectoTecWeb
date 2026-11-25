using System;
using System.Collections.Generic;
using System.Text;


namespace VinylStore.Core.DTOs
{
    public class SaledetailDto
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public int VinylId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}

