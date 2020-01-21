using Gallery.Web.Abstractions;
using Gallery.Web.Config;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Gallery.Web.Pages
{
    public class AlbumModel : GalleryPageModelBase<AlbumModel>
    {
        private readonly IAlbumService _albumService;

        public AlbumModel(ILogger<AlbumModel> logger, IAlbumService albumService,
            IAuthService authService, ITextMapService textMapService)
            : base(logger, authService, textMapService)
        {
            _albumService = albumService;
        }

        public Album Album { get; set; }

        [BindProperty]
        public string AlbumName { get; set; }

        public IEnumerable<UploadImage> FailedFiles { get; set; }

        public bool IsNotFound => (Album is null) || (!IsLoggedIn && Album.Visibility != Visibility.Public);

        [BindProperty]
        public bool IsVisible { get; set; }

        public override string PageName => "album";

        public override string PageTitle => "Album";

        public IEnumerable<UploadImage> SucceededFiles { get; set; }

        [BindProperty]
        public string UpdatedDescription { get; set; }

        public IActionResult OnGet(string albumName)
        {
            AlbumName = albumName;
            Album = _albumService.GetAlbumByName(albumName);

            if (IsNotFound)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Page();
            }

            UpdatedDescription = Album?.Description;
            IsVisible = ((Album?.Visibility) ?? Visibility.Private) == Visibility.Public;
            return Page();
        }

        public IActionResult OnPostAlbumInfo(string updatedDescription, bool isVisible)
        {
            if (!IsLoggedIn)
                return RedirectToPage(PageName);

            var album = _albumService.GetAlbumByName(AlbumName);
            if (album is null)
                return RedirectToPage(PageName);

            _albumService.UpdateAlbumInfo(AlbumName, updatedDescription,
                isVisible ? Visibility.Public : Visibility.Private);
            return RedirectToPage(PageName);
        }

        public async Task<IActionResult> OnPostUploadAsync(ICollection<IFormFile> files)
        {
            if (!IsLoggedIn)
                return RedirectToPage(PageName);

            var result = await _albumService.ProcessUploadFiles(files, AlbumName);
            SucceededFiles = result.Album.UploadImages.Values;
            FailedFiles = result.FailedFiles;
            return RedirectToPage(PageName);
        }
    }
}