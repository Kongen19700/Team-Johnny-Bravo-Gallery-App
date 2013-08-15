using Gallery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class ImageFullModel : ImageModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }

        public static ImageFullModel CreateFullImageModelFromEntity(Image entity)
        {
            return new ImageFullModel()
            {
                ImageId = entity.ImageId,
                Title = entity.Title,
                Url = entity.Url,
                AlbumId = entity.AlbumId,
                Comments = entity.Comments.Select(c => new CommentModel()
                {
                    CommentId = c.CommentId,
                    ImageId = c.ImageId,
                    Username = c.User.Username,
                    Text = c.Text,
                    UserId = c.UserId,
                }),
            };
        }
    }
}