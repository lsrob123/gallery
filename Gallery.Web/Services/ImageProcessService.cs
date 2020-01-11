using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gallery.Web.Services
{
    public class ImageProcessService : IImageProcessService
    {
        private readonly ILogger<ImageProcessService> _logger;

        public ImageProcessService(ILogger<ImageProcessService> logger)
        {
            _logger = logger;
        }

        public async Task<string> ResizeByHeightAsync(string sourceFilePath, string resizedFilePath,
            int resizedHeight, int quality = 100)
        {
            var resizedFileName = $"{Path.GetFileNameWithoutExtension(resizedFilePath)}.jpg";
            var folder = Path.GetDirectoryName(resizedFilePath);
            resizedFilePath = Path.Combine(folder, resizedFileName);
            if (File.Exists(resizedFilePath))
                File.Delete(resizedFilePath);

            using var sourceFileStream = File.OpenRead(sourceFilePath);
            using var memStream = new MemoryStream();
            try
            {
                await sourceFileStream.CopyToAsync(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                var bitmap = SKBitmap.Decode(memStream);

                var resizeInfo = new SKImageInfo(resizedHeight * bitmap.Width / bitmap.Height, resizedHeight);
                using var resizedBitmap = bitmap.Resize(resizeInfo, SKFilterQuality.High);
                using var newImg = SKImage.FromPixels(resizedBitmap.PeekPixels());
                using var data = newImg.Encode(SKEncodedImageFormat.Jpeg, quality);
                using var imgStream = data.AsStream();
                using var resizedFileStream = File.OpenWrite(resizedFilePath);
                imgStream.Seek(0, SeekOrigin.Begin);
                await imgStream.CopyToAsync(resizedFileStream);

                return resizedFilePath;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }
    }
}