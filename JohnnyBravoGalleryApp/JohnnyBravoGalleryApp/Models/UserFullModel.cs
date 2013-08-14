using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class UserFullModel : UserModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }
        public GalleryModel Gallery { get; set; }
    }
}