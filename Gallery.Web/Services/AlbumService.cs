using Gallery.Web.Abstractions;
using Gallery.Web.Config;
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
            _uploadImageRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath, _settings.AlbumRootPath);
            _logger = logger;
            _imageProcessService = imageProcessService;
            _albumRepository = albumRepository;
        }

        public void CreateAlbum(string name, string description, int dayOffset)
        {
            var album = new Album(name, description, _settings.DefaultThumbnailUriPathForAlbum, - Math.Abs(dayOffset));
            _albumRepository.UpdateAlbum(album);
        }

        public void DeleteAlbum(string albumName)
        {
            _albumRepository.DeleteAlbum(albumName);
        }

        public Album GetAlbumByName(string name)
        {
            return _albumRepository.GetAlbumByName(name);
        }

        public AlbumDaysCollection ListAlbumDays(Visibility visibility)
        {
            var albumDays = new AlbumDaysCollection();

            var albums = visibility == Visibility.All
                ? ListAlbums().OrderByDescending(x => x.DayCreated)
                : ListAlbums()
                    .Where(x => x.Visibility == Visibility.Public)
                    .OrderByDescending(x => x.DayCreated)
                ;

            if (!albums.Any())
                return albumDays;

            var daysOfAlbumsDisplayed = _settings.DaysOfAlbumsDisplayed;
            var daysIncluded = 0;
            foreach (var album in albums)
            {
                if (daysIncluded++ <= daysOfAlbumsDisplayed)
                {
                    albumDays.ForFullDetails.AddAlbum(album);
                }
                else
                {
                    albumDays.ForTextLinksOnly.AddAlbum(album);
                }
            }

            return albumDays;
        }

        public ICollection<Album> ListAlbums()
        {
            var albums = _albumRepository.ListAlbums()
                .RefreshThumbnailUris(_settings.DefaultThumbnailUriPathForAlbum);
            return albums;
        }

        public ICollection<Album> ListAlbumsByKeyword(string keyword)
        {
            var albums = _albumRepository.ListAlbumsByKeyword(keyword)
                .RefreshThumbnailUris(_settings.DefaultThumbnailUriPathForAlbum);
            return albums;
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
                var processedFile = new UploadImage(file, file.FileName, _settings.AlbumRootPath, result.Album.Name);

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
                        _settings.UploadImageThumbnailHeight);

                    await _imageProcessService.ResizeByHeightAsync(processedFilePath,
                        processedFile.GetIconFilePath(albumPath),
                        _settings.UploadImageIconHeight);

                    processedFiles.Add(processedFile);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    processedFile.MarkAsFailed();
                    result.FailedFiles.Add(processedFile);
                }
            }

            result.Album.WithUploadImages(processedFiles);
            _albumRepository.UpdateAlbum(result.Album);

            return result;
        }

        public void UpdateAlbum(Album album)
        {
            _albumRepository.UpdateAlbum(album);
        }

        public Album UpdateAlbumInfo(string name, string description, Visibility visibility)
        {
            var album = _albumRepository.GetAlbumByName(name)?.WithAlbumInfo(description, visibility);

            if (album is null)
                return null; //TODO: Add process result

            UpdateAlbum(album);
            return album;
        }

        public (Album Album, ProcessResult ProcessResult) UpdateUploadImageDisplayOrder
            (string albumName, string processedFileName, int displayOrder)
        {
            (Album Album, ProcessResult ProcessResult) result = (Album: null, ProcessResult: ProcessResult.Unknown);
            var album = _albumRepository.GetAlbumByName(albumName);
            if (album is null||!album.HasUploadImages|| 
                !album.UploadImages.TryGetValue(processedFileName, out var uploadImage))
            {
                result.ProcessResult = ProcessResult.NotFound;
                return result;
            }

            uploadImage.WithDisplayOrder(displayOrder);
            UpdateAlbum(album);

            result.Album = album;
            result.ProcessResult = ProcessResult.OK;
            return result;
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