using System;
using System.Collections.Generic;
using System.Text;
using VinylStore.Core.Entities;

namespace VinylStore.Core.Interfaces
{
    public interface IVinylRepository
    {
        Task InsertVinyl(Vinyl vinyl);
    }
}