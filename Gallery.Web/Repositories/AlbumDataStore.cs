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
            var connectionString = Path.Combine(Directory.GetCurrentDirectory(), settings.ConnectionString);
            _db = new LiteDatabase(connectionString);

            Albums = _db.GetCollection<Album>(nameof(Album));
            Albums.EnsureIndex(x => x.Name, true);
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
