using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json.Serialization;

namespace Gallery.Web.Models
{
    public class UploadImageModel
    {
        public UploadImageModel(IFormFile formFile, string processedFileName, string albumName,
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
        }

        [JsonIgnore]
        public IFormFile FormFile { get; }

        public bool IsSuccess { get; protected set; }
        public string OriginalFileName => FormFile?.Name;
        public string ProcessedFileName { get; protected set; }
        public string ThumbnailFileName { get; protected set; }
        public string Uri { get; protected set; }

        public string GetProcessedFilePath(string rootPath)
        {
            return Path.Combine(rootPath, ProcessedFileName);
        }

        public string GetThumbnailFilePath(string rootPath)
        {
            return Path.Combine(rootPath, ThumbnailFileName);
        }

        public UploadImageModel MarkAsFailed()
        {
            IsSuccess = false;
            ProcessedFileName = null;
            ThumbnailFileName = null;
            Uri = null;
            return this;
        }

        public UploadImageModel MarkAsSucceeded()
        {
            IsSuccess = true;
            return this;
        }

        public UploadImageModel WithinAlbum(string albumName)
        {
            Uri = $"{albumName.Trim().TrimStart('/').TrimEnd('/')}/{ProcessedFileName}";
            return this;
        }
    }
}