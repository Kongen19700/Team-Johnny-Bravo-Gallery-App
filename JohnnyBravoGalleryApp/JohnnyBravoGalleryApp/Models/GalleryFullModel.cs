using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class GalleryFullModel
    {
        public ICollection<AlbumModel> Albums { get; set; }
        public UserModel User { get; set; }
        public ICollection<ImageModel> Images { get; set; }
    }
}