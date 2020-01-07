using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Pages
{
    public class UploadModel : PageModel
    {
        private readonly ILogger<PageModel> _logger;

        public UploadModel(ILogger<PageModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostUpload(ICollection<IFormFile> files)
        {
            if ((files is null) || !files.Any())
                return Page();

            foreach (var file in files)
            {
                _logger.LogInformation(file.FileName);
            }

            return Page();
        }
    }
}
