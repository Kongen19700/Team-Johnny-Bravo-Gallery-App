using Gallery.Data;
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

        public static CommentModel CreateCommentModelFromEntity(Comment comment)
        {
            return new CommentModel()
            {
                CommentId = comment.CommentId,
                Text = comment.Text,
                UserId = comment.UserId,
                Username = comment.User.Username,
                ImageId = comment.ImageId,
            };
        }
    }
}
