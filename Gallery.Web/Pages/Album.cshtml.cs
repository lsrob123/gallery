using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Gallery.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Web.Pages
{
    public class AlbumModel : GalleryPageModelBase<AlbumModel>
    {
        private readonly IUploadImageService _uploadImageService;

        public AlbumModel(ILoggerFactory loggerFactory, IUploadImageService uploadImageService,
            IAuthService authService, ITextMapService textMapService)
            : base(loggerFactory, authService, textMapService)
        {
            _uploadImageService = uploadImageService;
        }

        [BindProperty]
        public string AlbumName { get; set; }

        public IEnumerable<UploadImageModel> UploadResults { get; set; }

        public void OnGet(string albumName)
        {
            AlbumName = T.GetMap(albumName ?? "_test");
        }

        public async Task OnPostUploadAsync(ICollection<IFormFile> files)
        {
            UploadResults = await _uploadImageService.ProcessUploadFilesAsync(files, AlbumName);
        }
    }
}