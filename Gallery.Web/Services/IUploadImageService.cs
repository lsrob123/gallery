using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Gallery.Web.Services
{
    public interface IUploadImageService
    {
        Task<IEnumerable<UploadImageModel>> ProcessUploadFilesAsync(ICollection<IFormFile> files, string albumName);
    }
}