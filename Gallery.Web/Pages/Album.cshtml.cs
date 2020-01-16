using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Pages
{
    public class AlbumModel : GalleryPageModelBase<AlbumModel>
    {
        private readonly IAlbumService _uploadImageService;

        public AlbumModel(ILogger<AlbumModel> logger, IAlbumService uploadImageService,
            IAuthService authService, ITextMapService textMapService)
            : base(logger, authService, textMapService)
        {
            _uploadImageService = uploadImageService;
        }

        [BindProperty]
        public string AlbumName { get; set; }

        public IEnumerable<UploadImage> UploadResults { get; set; }

        public void OnGet(string albumName)
        {
            AlbumName = T.GetMap(albumName ?? "_test");
        }

        public async Task OnPostUploadAsync(ICollection<IFormFile> files)
        {
            var album = await _uploadImageService.ProcessUploadFiles(files, AlbumName);
            UploadResults = album.UploadImages.Values;
            //TODO Display succeded or failed uploads
        }
    }
}