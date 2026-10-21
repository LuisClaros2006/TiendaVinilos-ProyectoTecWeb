using System;
using System.Collections.Generic;

namespace VinylStore.Core.Entities;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vinyl> Vinyls { get; set; } = new List<Vinyl>();
}
