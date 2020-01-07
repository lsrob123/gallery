using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Web.Config;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISettings _settings;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ISettings settings, IPasswordHasher<UserModel> passwordHasher, IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger)
        {
            _settings = settings;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public bool VerifyHashedPassword(string plainTextPassword)
        {
            UserModel userModel = new UserModel { PlainTextPassword = plainTextPassword };
            var result = _passwordHasher.VerifyHashedPassword(userModel, _settings.HashedPassword, plainTextPassword);
            return result == PasswordVerificationResult.Success;
        }

        public string HashPassword(string plainTextPassword)
        {
            var hash = _passwordHasher.HashPassword(new UserModel { PlainTextPassword = plainTextPassword }, plainTextPassword);
            return hash;
        }

        public async Task<bool> SignInAsync(string plainTextPassword)
        {
            try
            {
                var loginSucceeded = VerifyHashedPassword(plainTextPassword);
                if (!loginSucceeded)
                    throw new Exception("Wrong password");

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, new UserModel().Username)); //TODO: Change name getting

                var principle = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties { IsPersistent = false };
                await _httpContextAccessor.HttpContext.SignInAsync(principle, properties);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await SignOutAsync();
                return false;
            }
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsLoggedIn()
        {
            return !(_httpContextAccessor.HttpContext.User is null) &&
                !(_httpContextAccessor.HttpContext.User.Identity is null) &&
                _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetUser()
        {
            if (!IsLoggedIn())
                return null;

            return _httpContextAccessor.HttpContext.User.Identity.Name?? "unnamed";
        }
    }
}
