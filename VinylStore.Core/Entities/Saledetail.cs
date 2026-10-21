using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Saledetail
{
    public int SaleDetailId { get; set; }

    public int SaleId { get; set; }

    public int VinylId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Sale Sale { get; set; } = null!;

    public virtual Vinyl Vinyl { get; set; } = null!;
}
