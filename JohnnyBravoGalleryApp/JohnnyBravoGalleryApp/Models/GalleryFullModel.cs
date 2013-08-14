using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class GalleryFullModel : GalleryModel
    {
        public IEnumerable<AlbumModel> Albums { get; set; }
        public UserModel User { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }
    }
}