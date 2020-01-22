using Gallery.Web.Abstractions;
using Gallery.Web.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.Web.Models
{
    public class Album : EntityBase
    {
        public Album()
        {
        }

        public Album(string name, string description, string defaultThumbnailUriPath)
        {
            Name = name;
            WithAlbumInfo(description, Visibility.Public);
            DefaultThumbnailUriPath = ThumbnailUriPath = defaultThumbnailUriPath;
            SetKey(Guid.NewGuid());

            WithTimeUpdated(DateTimeOffset.UtcNow);
        }

        public DateTimeOffset DayUpdated => TimeUpdated.Date;
        public string DefaultThumbnailUriPath { get; protected set; }
        public string Description { get; set; }
        public bool HasUploadImages => !(UploadImages is null) && UploadImages.Any();
        public string InfoDisplay => HasUploadImages ? $"{UploadImages.Count} photos" : "(Empty)";
        public string Name { get; set; }
        public string ThumbnailUriPath { get; protected set; }
        public DateTimeOffset TimeUpdated { get; protected set; }
        public Dictionary<string, UploadImage> UploadImages { get; protected set; }
        public Visibility Visibility { get; protected set; }

        public void RefreshThumbnailUri(string defaultThumbnailUriPath = null)
        {
            if (!string.IsNullOrWhiteSpace(defaultThumbnailUriPath))
                DefaultThumbnailUriPath = defaultThumbnailUriPath;

            if ((UploadImages is null) || !UploadImages.Any())
            {
                ThumbnailUriPath = DefaultThumbnailUriPath;
            }
            else
            {
                ThumbnailUriPath = UploadImages.Values.OrderByDescending(x => x.TimeUpdated).First().UriPath;
            }
        }

        public Album RemoveUploadImage(string processedFileName, out string key)
        {
            key = processedFileName?.ToLower();
            if (key is null)
                return this;

            if (UploadImages.ContainsKey(key))
                UploadImages.Remove(key);

            RefreshThumbnailUri();
            return this;
        }

        public Album WithAlbumInfo(string description, Visibility visibility)
        {
            Description = description;
            Visibility = visibility;
            return this;
        }

        public Album WithTimeUpdated(DateTimeOffset dateTimeOffset)
        {
            TimeUpdated = dateTimeOffset;
            return this;
        }

        public Album WithUploadImage(UploadImage uploadImage)
        {
            return WithUploadImages(new UploadImage[] { uploadImage });
        }

        public Album WithUploadImages(IEnumerable<UploadImage> uploadImages)
        {
            if (UploadImages is null)
                UploadImages = new Dictionary<string, UploadImage>();

            foreach (var uploadImage in uploadImages)
            {
                RemoveUploadImage(uploadImage.ProcessedFileName, out string key);
                UploadImages.Add(key, uploadImage);
            }

            RefreshThumbnailUri();

            return this;
        }
    }
}