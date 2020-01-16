using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Gallery.Web.Abstractions;
using Gallery.Web.Config;

namespace Gallery.Web.Services
{
    public class TextMapService : ITextMapService
    {
        private readonly IDictionary<string, string> _maps;

        public TextMapService(ISettings settings)
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), settings.TextMapFilePath));
            _maps = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public string GetMap(string key)
        {
            if (_maps.TryGetValue(key, out var value))
                return value;

            return key;
        }
    }
}
