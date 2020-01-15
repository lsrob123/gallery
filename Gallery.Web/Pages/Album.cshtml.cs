using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Gallery.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Pages
{
    public class AlbumModel : GalleryPageModelBase
    {
        private readonly ILogger<AlbumModel> _logger;
        private readonly IUploadImageService _uploadImageService;
        private readonly ITextMapService _textMapService;

        public AlbumModel(ILogger<AlbumModel> logger, IUploadImageService uploadImageService,
            IAuthService authService, ITextMapService textMapService):base(authService,textMapService)
        {
            _logger = logger;
            _uploadImageService = uploadImageService;
            _textMapService = textMapService;
        }

        [BindProperty]
        public string AlbumName { get; set; }
        public IEnumerable<UploadImageModel> UploadResults { get; set; }

        public void OnGet(string albumName)
        {
            AlbumName = _textMapService.GetMap(albumName?? "_test");
        }

        public async Task OnPostUploadAsync(ICollection<IFormFile> files)
        {
            UploadResults = await _uploadImageService.ProcessUploadFilesAsync(files, AlbumName);
        }
    }
}
