using System;
using System.Collections.Generic;
using System.Linq;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ISettings _settings;
        private readonly ILogger<AlbumRepository> _logger;

        public AlbumRepository(ISettings settings, ILogger<AlbumRepository> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public IEnumerable<Album> ListAlbums()
        {
            try
            {
                using var store = new AlbumDataStore(_settings);
                var albums = store.Albums.FindAll().OrderBy(x => x.Name);
                return albums;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new Album[] { };
            }
        }

        public IEnumerable<Album> ListAlbumsByKeyword(string keyword)
        {
            try
            {
                keyword = keyword.ToLower();
                using var store = new AlbumDataStore(_settings);
                var albums = store.Albums
                    .Find(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword))
                    .OrderBy(x => x.Name);
                return albums;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new Album[] { };
            }
        }

        public void UpdateAlbum(Album album)
        {
            try
            {
                using var store = new AlbumDataStore(_settings);
                store.Albums.Delete(x => x.Name.ToLower() == album.Name.ToLower());
                store.Albums.Insert(album);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        public void DeleteAlbum(string albumName)
        {
            try
            {
                using var store = new AlbumDataStore(_settings);
                store.Albums.Delete(x => x.Name.ToLower() == albumName.ToLower());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        public Album GetAlbumByName(string name)
        {
            try
            {
                name = name.ToLower();
                using var store = new AlbumDataStore(_settings);
                var album = store.Albums
                    .FindOne(x => x.Name.ToLower() == name.ToLower());
                return album;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }
    }
}
