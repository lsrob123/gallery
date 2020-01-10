using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Web.Models;
using Gallery.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Pages
{
    public class AlbumModel : PageModel
    {
        private readonly ILogger<PageModel> _logger;
        private readonly IUploadImageService _uploadImageService;

        public AlbumModel(ILogger<PageModel> logger, IUploadImageService uploadImageService)
        {
            _logger = logger;
            _uploadImageService = uploadImageService;
        }

        public string AlbumName { get; set; }
        public IEnumerable<UploadImageModel> UploadResults { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostUploadAsync(ICollection<IFormFile> files)
        {
            //TODO: Validate album name
            UploadResults = await _uploadImageService.ProcessUploadFilesAsync(files, AlbumName);
        }
    }
}
