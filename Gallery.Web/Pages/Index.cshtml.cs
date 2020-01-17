using System.Collections.Generic;
using System.Linq;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IEnumerable<Album> Albums { get; private set; }

        public string CreateAlbumButtonText => T.GetMap("Create Album");

        public void OnGet()
        {
            LoadAlbums();
        }

        private void LoadAlbums()
        {
            Albums = _albumService.ListAlbums().OrderByDescending(x => x.TimeUpdated);

            if (!IsLoggedIn)
            {
                Albums = Albums.Where(x => x.Visible);
            }
        }

        public void OnPostCreateAlbum(string albumName, string albumDescription)
        {
            if (!IsLoggedIn)
                return;

            _albumService.CreateAlbum(albumName, albumDescription);
            LoadAlbums();
        }
    }
}