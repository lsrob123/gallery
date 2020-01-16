using System;
using Gallery.Web.Abstractions;

namespace Gallery.Web.Services
{
    public static class Extensions
    {
        public static T WithKey<T>(this T entity, Guid? key = null)
            where T : EntityBase
        {
            key ??= Guid.NewGuid();

            entity.SetKey(key.Value);
            return entity;
        }
    }
}
