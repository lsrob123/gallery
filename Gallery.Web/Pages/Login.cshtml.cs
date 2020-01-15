using Gallery.Web.Abstractions;
using Gallery.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gallery.Web.Pages
{
    public class LoginModel : GalleryPageModelBase<LoginModel>
    {
        public string LogIn => T.GetMap("Log In");
        public string LogOut => T.GetMap("Log Out");

        public LoginModel(ILoggerFactory loggerFactory, IAuthService authService, ITextMapService textMapService)
            : base(loggerFactory, authService, textMapService)
        {
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoginAsync(string password)
        {
            await AuthService.SignInAsync(password);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await AuthService.SignOutAsync();
            return RedirectToPage();
        }
    }
}