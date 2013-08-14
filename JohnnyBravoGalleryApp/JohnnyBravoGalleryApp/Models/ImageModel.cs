using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class ImageModel
    {
        public int ImageId { get; set; }
        public Nullable<int> AlbumId { get; set; }
        public Nullable<int> GalleryId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}