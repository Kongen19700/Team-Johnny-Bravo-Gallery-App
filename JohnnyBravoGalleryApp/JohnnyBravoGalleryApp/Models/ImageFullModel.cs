using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class ImageFullModel : ImageModel
    {
        public AlbumModel Album { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
        public GalleryModel Gallery { get; set; }
    }
}