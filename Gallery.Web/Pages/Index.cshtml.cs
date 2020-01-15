using Gallery.Web.Abstractions;
using Gallery.Web.Services;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Pages
{
    public class IndexModel : GalleryPageModelBase<IndexModel>
    {
        public IndexModel(ILoggerFactory loggerFactory, IAuthService authService, ITextMapService textMapService)
           : base(loggerFactory, authService, textMapService)
        {
        }

        public void OnGet()
        {
        }
    }
}