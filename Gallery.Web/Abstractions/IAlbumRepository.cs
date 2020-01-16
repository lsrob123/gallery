using System.Collections.Generic;
using Gallery.Web.Models;

namespace Gallery.Web.Abstractions
{
    public interface IAlbumRepository
    {
        void DeleteAlbum(string albumName);
        IEnumerable<Album> ListAlbums();
        Album GetAlbumByName(string name);
        IEnumerable<Album> ListAlbumsByKeyword(string keyword);
        void UpdateAlbum(Album album);
    }
}