using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class ImageUploadModel
    {
        public int ImageId { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}
