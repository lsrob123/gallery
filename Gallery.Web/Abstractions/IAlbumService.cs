using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Gallery.Web.Abstractions
{
    public interface IAlbumService
    {
        void DeleteAlbum(string albumName);
        IEnumerable<Album> ListAlbums();
        Album GetAlbumByName(string name);
        IEnumerable<Album> ListAlbumsByKeyword(string keyword);
        void UpdateAlbum(Album album);
        Task<Album> ProcessUploadFiles(ICollection<IFormFile> files, string albumName);
        Album CreateAlbum(string name, string description);
    }
}