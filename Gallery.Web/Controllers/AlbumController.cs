﻿using Gallery.Web.Abstractions;
using Gallery.Web.Config;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Controllers
{
    [ApiController]
    [Route("album")]
    public class AlbumController: ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IAlbumService _albumService;

        public AlbumController(ILogger<AlbumController> logger, IAlbumService albumService)
        {
            _logger = logger;
            _albumService = albumService;
        }

        [HttpPut]
        [Route("{albumName}/image/{processedFileName}")]
        public IActionResult UpdateUploadImageDisplayOrder([FromRoute]string albumName, 
            [FromRoute]string processedFileName, [FromBody] UpdateUploadImageDisplayOrderRequest request)
        {
            var result = _albumService.UpdateUploadImageDisplayOrder(albumName, processedFileName, request.DisplayOrder);
            return result.ProcessResult switch
            {
                ProcessResult.NotFound => NotFound(),
                _ => Ok(),
            };
        }
    }
}
