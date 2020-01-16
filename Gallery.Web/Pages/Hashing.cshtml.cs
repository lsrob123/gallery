using Gallery.Web.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Web.Pages
{
    public class HashingModel : PageModel
    {
        private readonly IAuthService _authService;

        public HashingModel(IAuthService authService)
        {
            _authService = authService;
        }

        public string HashedPassword { get; set; }

        public void OnGet()
        {
        }

        public void OnPostHash(string password)
        {
            HashedPassword = _authService.HashPassword(password);
        }
    }
}
