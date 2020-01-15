using Gallery.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Web.Abstractions
{
    public abstract class GalleryPageModelBase : PageModel
    {
        protected readonly IAuthService AuthService;
        protected readonly ITextMapService T;

        protected GalleryPageModelBase(IAuthService authService, ITextMapService textMapService)
        {
            AuthService = authService;
            T = textMapService;
        }

        public bool IsLoggedIn => AuthService.IsLoggedIn();
        public string UserName => AuthService.GetUser();
    }
}
