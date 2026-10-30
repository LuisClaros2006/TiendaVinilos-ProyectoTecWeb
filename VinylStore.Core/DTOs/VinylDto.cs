using System;
using System.Collections.Generic;
using System.Text;

namespace VinylStore.Core.DTOs
{
    public class VinylDto
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int GenreId { get; set; }

        public string Sku { get; set; } = string.Empty;

        public string Format { get; set; } = "LP";

        public string? Edition { get; set; }

        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
