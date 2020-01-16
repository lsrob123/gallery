using System;
using System.IO;
using System.Text.Json.Serialization;
using Gallery.Web.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Gallery.Web.Models
{
    public class UploadImage
    {
        public UploadImage(IFormFile formFile, string processedFileName, string albumName,
            string thumbnailFileName = null)
        {
            FormFile = formFile;
            ProcessedFileName = processedFileName.Replace('_', '-').Replace(' ', '-');
            WithinAlbum(albumName);
            if (string.IsNullOrWhiteSpace(ProcessedFileName))
                return;

            ThumbnailFileName = thumbnailFileName;
            if (!string.IsNullOrWhiteSpace(ThumbnailFileName))
                return;

            ThumbnailFileName = $"{Path.GetFileNameWithoutExtension(ProcessedFileName)}-small.jpg";
            TimeUpdated = DateTimeOffset.UtcNow;
        }

        [JsonIgnore]
        public IFormFile FormFile { get; }

        public bool IsSuccess { get; protected set; }
        public string OriginalFileName => FormFile?.Name;
        public string ProcessedFileName { get; protected set; }
        public string ThumbnailFileName { get; protected set; }
        public string Uri { get; protected set; }
        public string Description { get; protected set; }
        public DateTimeOffset TimeUpdated { get; protected set; }
        public int SequenceNumber { get; protected set; }

        public UploadImage WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public UploadImage WithSequenceNumber(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
            return this;
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
            Uri = null;
            return this;
        }

        public UploadImage MarkAsSucceeded()
        {
            IsSuccess = true;
            return this;
        }

        public UploadImage WithinAlbum(string albumName)
        {
            Uri = $"{albumName.Trim().TrimStart('/').TrimEnd('/')}/{ProcessedFileName}";
            return this;
        }
    }
}