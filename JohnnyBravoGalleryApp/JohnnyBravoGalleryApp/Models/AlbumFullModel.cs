using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class AlbumFullModel : AlbumModel
    {
        public ICollection<AlbumModel> SubAlbums { get; set; }
        public AlbumModel ParentAlbum { get; set; }
        public GalleryModel Gallery { get; set; }
        public ICollection<ImageModel> Images { get; set; }
    }
}