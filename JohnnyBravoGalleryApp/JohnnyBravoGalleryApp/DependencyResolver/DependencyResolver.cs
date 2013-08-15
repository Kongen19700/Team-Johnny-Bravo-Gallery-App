using Gallery.Data;
using Gallery.Repositories;
using JohnnyBravoGalleryApp.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace JohnnyBravoGalleryApp.DependencyResolver
{
    public class DbDependencyResolver : IDependencyResolver
    {
        private static DbContext context = new GalleryEntities();
        private static IRepository<User> userRepo = new EfRepository<User>(context);
        private static IRepository<Image> imageRepo = new EfRepository<Image>(context);
        private static IRepository<Album> albumRepo = new EfRepository<Album>(context);
        private static IRepository<Comment> comentRepo = new EfRepository<Comment>(context);

        private readonly AllRepositories repos =
            new AllRepositories(albumRepo, imageRepo, userRepo, comentRepo);

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(GalleriesController))
            {
                return new GalleriesController(repos);
            }
            else if (serviceType == typeof(ImagesController))
            {
                return new ImagesController(repos);
            }
            else if (serviceType == typeof(ImageController))
            {
                return new ImageController(repos);
            }
            else if (serviceType == typeof(UsersController))
            {
                return new UsersController(repos);
            }
            else if (serviceType == typeof(CommentsController))
            {
                return new CommentsController(repos);
            }
            else if (serviceType == typeof(AlbumsController))
            {
                return new AlbumsController(repos);
            }
            else
            {
                return null;
            }
        }

        public static AllRepositories GetAllRepositories()
        {
            return new AllRepositories(
                new EfRepository<Album>(new GalleryEntities()),
                new EfRepository<Image>(new GalleryEntities()),
                new EfRepository<User>(new GalleryEntities()),
                new EfRepository<Comment>(new GalleryEntities()));
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}