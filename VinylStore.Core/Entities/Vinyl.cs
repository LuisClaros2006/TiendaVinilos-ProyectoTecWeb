using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Vinyl
{
    public int VinylId { get; set; }

    public int AlbumId { get; set; }

    public int GenreId { get; set; }

    public string Sku { get; set; } = null!;

    public string Format { get; set; } = null!;

    public string? Edition { get; set; }

    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Album Album { get; set; } = null!;

    [JsonIgnore]
    public virtual Genre Genre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Saledetail> Saledetails { get; set; } = new List<Saledetail>();
}