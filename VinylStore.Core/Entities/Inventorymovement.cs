using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Inventorymovement : BaseEntity
{
    // public int Id { get; set; }

    public int VinylId { get; set; }

    public int? EmployeeId { get; set; }

    public int? SaleId { get; set; }

    public string MovementType { get; set; } = null!;

    public int QuantityChange { get; set; }

    public int StockBefore { get; set; }

    public int StockAfter { get; set; }

    public string? Reason { get; set; }

    public DateTime MovementDate { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Sale? Sale { get; set; }

    public virtual Vinyl Vinyl { get; set; } = null!;
}
