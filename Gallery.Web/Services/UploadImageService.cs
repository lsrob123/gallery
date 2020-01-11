using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Web.Config;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Services
{
    public class UploadImageService : IUploadImageService
    {
        private readonly string _uploadImageRootPath;
        private readonly ISettings _settings;
        private readonly ILogger<UploadImageService> _logger;
        private readonly IImageProcessService _imageProcessService;

        public UploadImageService(IWebHostEnvironment env, ISettings settings, ILogger<UploadImageService> logger,
            IImageProcessService imageProcessService)
        {
            _settings = settings;
            _uploadImageRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath);
            _logger = logger;
            _imageProcessService = imageProcessService;
        }

        public async Task<IEnumerable<UploadImageModel>> ProcessUploadFilesAsync(ICollection<IFormFile> files, 
            string albumName)
        {
            var results = new List<UploadImageModel>();
            if ((files is null) || !files.Any())
                return results;

            if (string.IsNullOrWhiteSpace(albumName))
                albumName = string.Empty;
            var albumPath = Path.Combine(_uploadImageRootPath, albumName);

            if (!Directory.Exists(albumPath))
                Directory.CreateDirectory(albumPath);

            foreach (var file in files)
            {
                var processedFileModel = new UploadImageModel(file, file.FileName, albumName);

                var processedFilePath = processedFileModel.GetProcessedFilePath(albumPath);
                CheckFileExistence(albumPath, processedFilePath, processedFileModel);

                try
                {
                    using (var fileStream = new FileStream(processedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    processedFileModel.MarkAsSucceeded();

                    await _imageProcessService.ResizeByHeightAsync(processedFilePath,
                        processedFileModel.GetThumbnailFilePath(albumPath),
                        _settings.ThumbnailHeight);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    processedFileModel.MarkAsFailed();
                }

                results.Add(processedFileModel);
            }

            return results;
        }

        private static void CheckFileExistence(string albumPath, string processedFilePath, 
            UploadImageModel processedFileModel)
        {
            if (File.Exists(processedFilePath))
            {
                File.Delete(processedFilePath);
            }
            var thumbnailFilePath = processedFileModel.GetThumbnailFilePath(albumPath);
            if (File.Exists(thumbnailFilePath))
            {
                File.Delete(thumbnailFilePath);
            }
        }
    }
}
