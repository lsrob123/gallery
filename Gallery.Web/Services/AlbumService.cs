using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly string _uploadImageRootPath;
        private readonly ISettings _settings;
        private readonly ILogger<AlbumService> _logger;
        private readonly IImageProcessService _imageProcessService;
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IWebHostEnvironment env, ISettings settings, ILogger<AlbumService> logger,
            IImageProcessService imageProcessService, IAlbumRepository albumRepository)
        {
            _settings = settings;
            _uploadImageRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath);
            _logger = logger;
            _imageProcessService = imageProcessService;
            _albumRepository = albumRepository;
        }

        public async Task<Album> ProcessUploadFiles(ICollection<IFormFile> files, string albumName)
        {
            var album = GetAlbumByName(albumName);
            if (album is null)
                return null;

            var albumPath = Path.Combine(_uploadImageRootPath, album.Name);

            if (!Directory.Exists(albumPath))
                Directory.CreateDirectory(albumPath);

            var processedFiles = new List<UploadImage>();
            foreach (var file in files)
            {
                var processedFile = new UploadImage(file, file.FileName, album.Name);

                var processedFilePath = processedFile.GetProcessedFilePath(albumPath);
                CheckFileExistence(albumPath, processedFilePath, processedFile);

                try
                {
                    using (var fileStream = new FileStream(processedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    processedFile.MarkAsSucceeded();

                    await _imageProcessService.ResizeByHeightAsync(processedFilePath,
                        processedFile.GetThumbnailFilePath(albumPath),
                        _settings.ThumbnailHeight);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    processedFile.MarkAsFailed();
                }

                processedFiles.Add(processedFile);
            }

            album.WithUploadImages(processedFiles);
            _albumRepository.UpdateAlbum(album);

            return album;
        }

        private static void CheckFileExistence(string albumPath, string processedFilePath,
            UploadImage processedFile)
        {
            if (File.Exists(processedFilePath))
            {
                File.Delete(processedFilePath);
            }
            var thumbnailFilePath = processedFile.GetThumbnailFilePath(albumPath);
            if (File.Exists(thumbnailFilePath))
            {
                File.Delete(thumbnailFilePath);
            }
        }

        public void DeleteAlbum(string albumName)
        {
            _albumRepository.DeleteAlbum(albumName);
        }

        public IEnumerable<Album> ListAlbums()
        {
            return _albumRepository.ListAlbums();
        }

        public IEnumerable<Album> ListAlbumsByKeyword(string keyword)
        {
            return _albumRepository.ListAlbumsByKeyword(keyword);
        }

        public void UpdateAlbum(Album album)
        {
            _albumRepository.UpdateAlbum(album);
        }

        public Album GetAlbumByName(string name)
        {
            return _albumRepository.GetAlbumByName(name);
        }

        public Album CreateAlbum(string name, string description)
        {
            var album = new Album
            {
                Name = name,
                Description = description
            }.WithKey();
            return album;
        }
    }
}
