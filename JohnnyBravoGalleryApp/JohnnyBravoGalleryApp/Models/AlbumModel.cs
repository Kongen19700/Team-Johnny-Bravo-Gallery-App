using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JohnnyBravoGalleryApp.Models
{
    public class AlbumModel
    {
        public int AlbumId { get; set; }
        public int GalleryId { get; set; }
        public Nullable<int> ParentAlbumId { get; set; }
        public string Title { get; set; }
    }
}
