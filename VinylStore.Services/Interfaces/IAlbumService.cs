using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Entities;

namespace VinylStore.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album?> GetAlbumByIdAsync(int id);
        Task InsertAlbum(Album album);
        Task UpdateAlbum(Album album);
        Task DeleteAlbum(int id);
    }
}