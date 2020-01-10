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
        private readonly ILogger<UploadImageService> _logger;

        public UploadImageService(IWebHostEnvironment env, ISettings settings, ILogger<UploadImageService> logger)
        {
            _uploadImageRootPath = Path.Combine(env.WebRootPath, settings.UploadImageRootPath);
            _logger = logger;
        }

        public async Task<IEnumerable<UploadImageModel>> ProcessUploadFilesAsync(ICollection<IFormFile> files, string albumName)
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
                _logger.LogInformation(file.FileName);

                var processedFileName = $"{albumName}-{file.Name}";
                var processedFilePath = Path.Combine(albumPath, processedFileName);
                if (File.Exists(processedFilePath))
                {
                    //TODO: Add suffix to file name 
                }

                var result = new UploadImageModel(file, processedFileName, albumName);

                try
                {
                    using (var fileStream = new FileStream(processedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                        result = result.AsSuccessful();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    result = result.AsFailed();
                }

                results.Add(result);
            }

            return results;
        }
    }
}
