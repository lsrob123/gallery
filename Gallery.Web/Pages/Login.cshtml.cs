﻿using System.Threading.Tasks;
using Gallery.Web.Abstractions;
using Gallery.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Web.Pages
{
    public class LoginModel : GalleryPageModelBase
    {
        public string LogIn => T.GetMap("Log In");
        public string LogOut => T.GetMap("Log Out");

        public LoginModel(IAuthService authService, ITextMapService textMapService):base(authService, textMapService)
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
