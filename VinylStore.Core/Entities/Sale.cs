using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Sale
{
    public int SaleId { get; set; }

    public int CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime SaleDate { get; set; }

    public decimal Total { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CancelledAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<Saledetail> Saledetails { get; set; } = new List<Saledetail>();
}
