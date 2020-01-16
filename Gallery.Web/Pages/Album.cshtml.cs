using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [BindProperty]
        public IEnumerable<UploadImage> FailedFiles { get; set; }

        public IEnumerable<UploadImage> SucceededFiles { get; set; }

        public void OnGet(string albumName)
        {
            AlbumName = T.GetMap(albumName ?? "_test");
        }

        public async Task OnPostUploadAsync(ICollection<IFormFile> files)
        {
            var result = await _uploadImageService.ProcessUploadFiles(files, AlbumName);
            SucceededFiles = result.Album.UploadImages.Values;
            FailedFiles = result.FailedFiles;
        }
    }
}