using Gallery.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gallery.Web.Pages
{
    public class LoginModel : GalleryPageModelBase<LoginModel>
    {
        public LoginModel(ILogger<LoginModel> logger, IAuthService authService, ITextMapService textMapService)
            : base(logger, authService, textMapService)
        {
        }

        public string LogIn => "Log In";
        public string LogOut => "Log Out";
        public override string PageName => "Login";
        public override string PageTitle => "Login";

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