using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Gallery.Web.Models
{
    public class UploadImageModel
    {
        public UploadImageModel(IFormFile formFile, string processedFileName, string albumName, string thumbnailFileName = null)
        {
            FormFile = formFile;
            ProcessedFileName = processedFileName;
            WithinAlbum(albumName);
            if (string.IsNullOrWhiteSpace(ProcessedFileName))
                return;

            ThumbnailFileName = thumbnailFileName;
            if (string.IsNullOrWhiteSpace(ThumbnailFileName))
                ThumbnailFileName = $"_tn_${ProcessedFileName}";
        }

        public bool IsSuccess { get; protected set; }
        [JsonIgnore]
        public IFormFile FormFile { get; }
        public string OriginalFileName => FormFile?.Name;
        public string ProcessedFileName { get; protected set; }
        public string ThumbnailFileName { get; protected set; }
        public string Uri { get; protected set; }

        public UploadImageModel WithinAlbum(string albumName)
        {
            Uri = $"{albumName.Trim().TrimStart('/').TrimEnd('/')}/{ProcessedFileName}";
            return this;
        }

        public UploadImageModel AsSuccessful()
        {
            IsSuccess = true;
            return this;
        }

        public UploadImageModel AsFailed()
        {
            IsSuccess = false;
            ProcessedFileName = null;
            ThumbnailFileName = null;
            Uri = null;
            return this;
        }
    }
}
