using System;
using System.Collections.Generic;
using System.Linq;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;

namespace Gallery.Web.Services
{
    public static class Extensions
    {
        public static ICollection<Album> RefreshThumbnailUris(this ICollection<Album> albums,
            string defaultThumbnailUriPathForAlbum)
        {
            foreach (var album in albums)
            {
                album.RefreshThumbnailUri(defaultThumbnailUriPathForAlbum);
            }
            return albums;
        }

        public static T WithKey<T>(this T entity, Guid? key = null)
            where T : EntityBase
        {
            key ??= Guid.NewGuid();

            entity.SetKey(key.Value);
            return entity;
        }

        public static IDictionary<DateTimeOffset, List<Album>> AddAlbum
            (this IDictionary<DateTimeOffset, List<Album>> albums, Album album)
        {
            var key = album.DayCreated;
            if (albums.TryGetValue(key, out var albumsOfDay))
            {
                albumsOfDay.Add(album);
            }
            else
            {
                albumsOfDay = new List<Album> { album };
                albums.Add(key, albumsOfDay);
            }
            return albums;
        }

        public static bool HasAlbums
           (this IDictionary<DateTimeOffset, List<Album>> albums)
        {
            return !(albums is null) && albums.Any();
        }
    }
}