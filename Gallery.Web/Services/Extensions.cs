using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using System;
using System.Collections.Generic;

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
    }
}