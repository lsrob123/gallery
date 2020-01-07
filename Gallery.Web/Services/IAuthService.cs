﻿using System.Threading.Tasks;

namespace Gallery.Web.Services
{
    public interface IAuthService
    {
        string HashPassword(string plainTextPassword);
        bool VerifyHashedPassword(string plainTextPassword);
        Task<bool> SignInAsync(string plainTextPassword);
        Task SignOutAsync();
        bool IsLoggedIn();
        string GetUser();
    }
}