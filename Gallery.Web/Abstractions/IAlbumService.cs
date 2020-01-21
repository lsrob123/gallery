﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Web.Config;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Gallery.Web.Abstractions
{
    public interface IAlbumService
    {
        void CreateAlbum(string name, string description);

        void DeleteAlbum(string albumName);

        Album GetAlbumByName(string name);

        ICollection<Album> ListAlbums();

        ICollection<Album> ListAlbumsByKeyword(string keyword);

        Task<(Album Album, ICollection<UploadImage> FailedFiles)>
            ProcessUploadFiles(ICollection<IFormFile> files, string albumName);

        void UpdateAlbum(Album album);

        Album UpdateAlbumInfo(string name, string description, Visibility visibility);

        IDictionary<DateTimeOffset, List<Album>> ListAlbumsDays(Visibility visibility);
    }
}