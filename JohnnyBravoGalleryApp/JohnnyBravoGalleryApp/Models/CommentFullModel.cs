using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class CommentFullModel : CommentModel
    {
        public ImageModel Image { get; set; }
        public UserModel User { get; set; }
    }
}