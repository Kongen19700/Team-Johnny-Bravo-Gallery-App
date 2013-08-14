using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JohnnyBravoGalleryApp.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int ImageId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
