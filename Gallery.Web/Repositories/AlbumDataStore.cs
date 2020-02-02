using System;
using System.IO;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using LiteDB;

namespace Gallery.Web.Repositories
{
    public class AlbumDataStore : IDisposable
    {
        private bool disposed = false;
        private readonly LiteDatabase _db;

        public AlbumDataStore(ISettings settings)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", settings.Database);
            var folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            _db = new LiteDatabase($"Filename={filePath}");

            Albums = _db.GetCollection<Album>(nameof(Album));
            Albums.EnsureIndex(x => x.Name, true);
            Albums.EnsureIndex(x => x.TimeCreated, false);
        }

        public LiteCollection<Album> Albums { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _db.Dispose();
            }

            disposed = true;
        }
    }
}
