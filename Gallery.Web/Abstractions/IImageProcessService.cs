using System.Threading.Tasks;

namespace Gallery.Web.Abstractions
{
    public interface IImageProcessService
    {
        Task<string> ResizeByHeightAsync(string sourceFilePath, string resizedFilePath, int resizedHeight, int quality = 90);
    }
}