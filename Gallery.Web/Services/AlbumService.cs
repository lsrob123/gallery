﻿using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Web.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IImageProcessService _imageProcessService;
        private readonly ILogger<AlbumService> _logger;
        private readonly ISettings _settings;
        private readonly string _uploadImageRootPath;

        public AlbumService(IWebHostEnvironment env, ISettings settings, ILogger<AlbumService> logger,
            IImageProcessService imageProcessService, IAlbumRepository albumRepository)
        {
            _settings = settings;
            _uploadImageRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath);
            _logger = logger;
            _imageProcessService = imageProcessService;
            _albumRepository = albumRepository;
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

        public void DeleteAlbum(string albumName)
        {
            _albumRepository.DeleteAlbum(albumName);
        }

        public Album GetAlbumByName(string name)
        {
            return _albumRepository.GetAlbumByName(name);
        }

        public IEnumerable<Album> ListAlbums()
        {
            return _albumRepository.ListAlbums();
        }

        public IEnumerable<Album> ListAlbumsByKeyword(string keyword)
        {
            return _albumRepository.ListAlbumsByKeyword(keyword);
        }

        public async Task<(Album Album, ICollection<UploadImage> FailedFiles)> 
            ProcessUploadFiles(ICollection<IFormFile> files, string albumName)
        {
            (Album Album, ICollection<UploadImage> FailedFiles) result =
                (Album: null, FailedFiles: new List<UploadImage>());

            result.Album = GetAlbumByName(albumName);
            if (result.Album is null)
                return result;

            var albumPath = Path.Combine(_uploadImageRootPath, result.Album.Name);

            if (!Directory.Exists(albumPath))
                Directory.CreateDirectory(albumPath);

            var processedFiles = new List<UploadImage>();
            foreach (var file in files)
            {
                var processedFile = new UploadImage(file, file.FileName, result.Album.Name);

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

                    processedFiles.Add(processedFile);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    processedFile.MarkAsFailed();
                    result.FailedFiles.Add(processedFile);
                }

                
            }

            result.Album.WithUploadImages(processedFiles.Where(x => x.IsSuccess));
            _albumRepository.UpdateAlbum(result.Album);

            return result;
        }

        public void UpdateAlbum(Album album)
        {
            _albumRepository.UpdateAlbum(album);
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
    }
}