using Gallery.Data;
using Gallery.Repositories;
using JohnnyBravoGalleryApp.DependencyResolver;
using JohnnyBravoGalleryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JohnnyBravoGalleryApp.Controllers
{
    public class ImageController : ApiController
    {
        private readonly AllRepositories repos;

        public ImageController()
        {
            this.repos = DbDependencyResolver.GetAllRepositories();
        }

        public ImageController(AllRepositories repos)
        {
            this.repos = repos;
        }

        public ImageFullModel GetImage(int id)
        {
            Image image = this.repos.ImageRepo.Get(id);

            if (image == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            ImageFullModel imageModel = ImageFullModel.CreateFullImageModelFromEntity(image);

            return imageModel;
        }
    }
}
