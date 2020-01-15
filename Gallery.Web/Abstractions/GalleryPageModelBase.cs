using Gallery.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Abstractions
{
    public abstract class GalleryPageModelBase<T> : GalleryPageModelBase
        where T : GalleryPageModelBase
    {
        protected readonly ILogger<T> Logger;

        public GalleryPageModelBase(ILoggerFactory loggerFactory, IAuthService authService, 
            ITextMapService textMapServic)
            : base(authService, textMapServic)
        {
            Logger = loggerFactory.CreateLogger<T>();
        }
    }

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