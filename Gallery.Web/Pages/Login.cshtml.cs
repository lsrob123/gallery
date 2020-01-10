using System.Threading.Tasks;
using Gallery.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ITextMapService _textMapService;

        public bool IsLoggedIn => _authService.IsLoggedIn();
        public string UserName => _authService.GetUser();

        public string LogIn => _textMapService.GetMap("Log In");
        public string LogOut => _textMapService.GetMap("Log Out");

        public LoginModel(IAuthService authService, ITextMapService textMapService)
        {
            _authService = authService;
            _textMapService = textMapService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoginAsync(string password)
        {
            await _authService.SignInAsync(password);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.SignOutAsync();
            return RedirectToPage();
        }
    }
}
