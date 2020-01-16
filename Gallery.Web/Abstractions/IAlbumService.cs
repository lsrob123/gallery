using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Web.Abstractions
{
    public interface IAlbumService
    {
        Album CreateAlbum(string name, string description);

        void DeleteAlbum(string albumName);

        Album GetAlbumByName(string name);

        IEnumerable<Album> ListAlbums();

        IEnumerable<Album> ListAlbumsByKeyword(string keyword);

        Task<(Album Album, ICollection<UploadImage> FailedFiles)>
            ProcessUploadFiles(ICollection<IFormFile> files, string albumName);

        void UpdateAlbum(Album album);
    }
}