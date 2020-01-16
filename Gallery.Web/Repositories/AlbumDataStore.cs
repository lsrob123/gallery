using System;
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
            _db = new LiteDatabase(settings.ConnectionString);

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
