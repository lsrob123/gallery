using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Abstractions
{
    public abstract class GalleryPageModelBase<T> : GalleryPageModelBase
        where T : GalleryPageModelBase
    {
        protected readonly ILogger<T> Logger;

        public GalleryPageModelBase(ILogger<T> logger, IAuthService authService,
            ITextMapService textMapServic)
            : base(authService, textMapServic)
        {
            Logger = logger;
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
        public abstract string PageName { get; }
        public abstract string PageTitle { get; }
    }
}