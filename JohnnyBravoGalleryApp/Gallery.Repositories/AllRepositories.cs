using Gallery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Repositories
{
    public class AllRepositories
    {
        public AllRepositories(
            IRepository<Album> albumRepo,
            IRepository<Image> imageRepo,
            IRepository<User> userRepo, 
            IRepository<Comment> commentRepo)
        {
            this.ImageRepo = imageRepo;
            this.AlbumRepo = albumRepo;
            this.UserRepo = userRepo;
            this.CommentRepo = commentRepo;
        }

        public IRepository<Image> ImageRepo { get; private set; }

        public IRepository<Album> AlbumRepo { get; private set; }

        public IRepository<User> UserRepo { get; private set; }

        public IRepository<Comment> CommentRepo { get; private set; }
    }
}
