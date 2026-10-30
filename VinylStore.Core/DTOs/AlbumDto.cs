using System;
using System.Collections.Generic;
using System.Text;

namespace VinylStore.Core.DTOs
{
    public class AlbumDto
    {
        public int Id { get; set; }

        public int ArtistId { get; set; }

        public string Title { get; set; } = string.Empty;

        public ushort? ReleaseYear { get; set; }
    }
}
