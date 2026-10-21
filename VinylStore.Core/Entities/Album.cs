using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Album
{
    public int AlbumId { get; set; }

    public int ArtistId { get; set; }

    public string Title { get; set; } = null!;

    public ushort? ReleaseYear { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Vinyl> Vinyls { get; set; } = new List<Vinyl>();
}
