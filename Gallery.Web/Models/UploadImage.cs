using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace Gallery.Web.Models
{
    public class UploadImage
    {
        public UploadImage()
        {
        }

        public UploadImage(IFormFile formFile, string processedFileName, string albumRootPath, string albumName)
        {
            FormFile = formFile;
            ProcessedFileName = processedFileName.Replace('_', '-').Replace(' ', '-').ToLower();
            if (string.IsNullOrWhiteSpace(ProcessedFileName))
                return;

            ThumbnailFileName = $"{Path.GetFileNameWithoutExtension(ProcessedFileName)}-small.jpg";
            IconFileName = $"{Path.GetFileNameWithoutExtension(ProcessedFileName)}-tiny.jpg";

            TimeUpdated = DateTimeOffset.UtcNow;

            WithinAlbum(albumRootPath, albumName);
        }

        public string Description { get; protected set; }

        public int DisplayOrder { get; protected set; }

        [JsonIgnore]
        public IFormFile FormFile { get; }

        public string IconFileName { get; protected set; }
        public string IconUriPath { get; protected set; }
        public bool IsSuccess { get; protected set; }
        public string OriginalFileName => FormFile?.Name;
        public string ProcessedFileName { get; protected set; }
        public string ThumbnailFileName { get; protected set; }
        public string ThumbnailUriPath { get; protected set; }
        public DateTimeOffset TimeUpdated { get; protected set; }
        public string UriPath { get; protected set; }

        public string GetIconFilePath(string rootPath)
        {
            return Path.Combine(rootPath, IconFileName);
        }

        public string GetProcessedFilePath(string rootPath)
        {
            return Path.Combine(rootPath, ProcessedFileName);
        }

        public string GetThumbnailFilePath(string rootPath)
        {
            return Path.Combine(rootPath, ThumbnailFileName);
        }

        public UploadImage MarkAsFailed()
        {
            IsSuccess = false;
            ProcessedFileName = null;
            ThumbnailFileName = null;
            UriPath = null;
            ThumbnailUriPath = null;
            return this;
        }

        public UploadImage MarkAsSucceeded()
        {
            IsSuccess = true;
            return this;
        }

        public UploadImage WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public UploadImage WithDisplayOrder(int sequenceNumber)
        {
            DisplayOrder = sequenceNumber;
            return this;
        }

        public UploadImage WithinAlbum(string albumRootPath, string albumName)
        {
            var rootPath = $"{albumRootPath.Trim().TrimStart('/').TrimEnd('/')}/{albumName.Trim().TrimStart('/').TrimEnd('/')}";
            UriPath = $"{rootPath}/{ProcessedFileName}";
            ThumbnailUriPath = $"{rootPath}/{ThumbnailFileName}";
            IconUriPath = $"{rootPath}/{IconFileName}";
            return this;
        }
    }
}