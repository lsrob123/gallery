using System;
using System.Collections.Generic;
using Gallery.Web.Services;

namespace Gallery.Web.Models
{
    public class AlbumDaysCollection
    {
        public IDictionary<DateTimeOffset, List<Album>> ForFullDetails { get; set; }
        public IDictionary<DateTimeOffset, List<Album>> ForTextLinksOnly { get; set; }

        public AlbumDaysCollection()
        {
            ForFullDetails = new Dictionary<DateTimeOffset, List<Album>>();
            ForTextLinksOnly = new Dictionary<DateTimeOffset, List<Album>>();
        }

        public bool HasAlbumsForFullDetails => ForFullDetails.HasAlbums();
        public bool HasAlbumsForTextLinksOnly => ForTextLinksOnly.HasAlbums();
    }
}
