using System;
using System.Collections.Generic;
using Gallery.Web.Abstractions;

namespace Gallery.Web.Models
{
    public class Album : EntityBase
    {
        public Album()
        {
            TimeUpdated = DateTimeOffset.UtcNow;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset TimeUpdated { get; protected set; }
        public Dictionary<string, UploadImage> UploadImages { get; set; }

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

            return this;
        }

        public Album RemoveUploadImage(string processedFileName, out string key)
        {
            key = processedFileName?.ToLower();
            if (key is null)
                return this;

            if (UploadImages.ContainsKey(key))
                UploadImages.Remove(key);
            return this;
        }
    }
}
