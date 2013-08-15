using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Gallery.Data;
using Gallery.Repositories;
using JohnnyBravoGalleryApp.Models;

namespace JohnnyBravoGalleryApp.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly AllRepositories repos;

        public ImagesController(AllRepositories repos)
        {
            this.repos = repos;
        }

        [HttpGet]
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

        [HttpPost]
        public HttpResponseMessage PostImage(Image image)
        {
            //TODO: Upload image to dropbox, add image entity to db
            
            //if (ModelState.IsValid)
            //{
            //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, image);
            //    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = image.ImageId }));
            //    return response;
            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest);
            //}

            throw new NotImplementedException();
        }
    }
}