using Gallery.Web.Abstractions;
using Gallery.Web.Config;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.Web.Pages
{
    public class IndexModel : GalleryPageModelBase<IndexModel>
    {
        private readonly IAlbumService _albumService;

        public IndexModel(ILogger<IndexModel> logger, IAuthService authService, ITextMapService textMapService,
            IAlbumService albumService)
           : base(logger, authService, textMapService)
        {
            _albumService = albumService;
        }

        [BindProperty]
        public IEnumerable<Album> Albums { get; private set; } = new List<Album>();

        public string CreateAlbumButtonText => T.GetMap("Create Album");

        public override string PageName => Constants.HomePageName;

        public override string PageTitle => Constants.HomePageTitle;

        public void OnGet()
        {
            LoadAlbums();
        }

        public IActionResult OnPostCreateAlbum(string albumName)
        {
            if (!IsLoggedIn)
                return Page();

            _albumService.CreateAlbum(albumName, null);
            LoadAlbums();
            return RedirectToPage(PageName);
        }

        private void LoadAlbums()
        {
            Albums = _albumService.ListAlbums();

            if (!IsLoggedIn)
            {
                Albums = Albums.Where(x => x.Visibility == Visibility.Visible);
            }

            Albums = Albums;
        }
    }
}