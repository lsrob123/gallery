using Gallery.Web.Abstractions;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Pages
{
    public class IndexModel : GalleryPageModelBase<IndexModel>
    {
        public IndexModel(ILogger<IndexModel> logger, IAuthService authService, ITextMapService textMapService)
           : base(logger, authService, textMapService)
        {
        }

        public void OnGet()
        {
        }
    }
}