using Microsoft.Extensions.Configuration;

namespace Gallery.Web.Config
{
    public class Settings : ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string HashedPassword => _configuration.GetValue<string>(nameof(HashedPassword));
        public string TextMapFilePath => _configuration.GetValue<string>(nameof(TextMapFilePath));
    }
}
