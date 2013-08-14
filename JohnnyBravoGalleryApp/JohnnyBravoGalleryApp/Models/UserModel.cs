using Gallery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JohnnyBravoGalleryApp.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public static UserModel CreateFullUserModelFromEntity(User entity)
        {
            return new UserModel()
            {
                UserId = entity.UserId,
                Username = entity.Username,
            };
        }
    }
}
