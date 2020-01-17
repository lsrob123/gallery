using Gallery.Web.Abstractions;
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
        public int ThumbnailHeight => _configuration.GetValue<int>(nameof(ThumbnailHeight));
        public string UploadImageRootPath => _configuration.GetValue<string>(nameof(UploadImageRootPath));
        public string ConnectionString => _configuration.GetValue<string>(nameof(ConnectionString));
        public string DefaultThumbnailUriPathForAlbum => _configuration.GetValue<string>(nameof(DefaultThumbnailUriPathForAlbum));
    }
}