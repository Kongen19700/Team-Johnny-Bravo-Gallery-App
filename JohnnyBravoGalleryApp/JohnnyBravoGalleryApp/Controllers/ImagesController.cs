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

namespace JohnnyBravoGalleryApp.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly AllRepositories repos;

        public ImagesController(AllRepositories repos)
        {
            this.repos = repos;
        }

        private GalleryEntities db = new GalleryEntities();

        // GET api/Images
        public IEnumerable<Image> GetImages()
        {
            var images = db.Images.Include(i => i.Album).Include(i => i.Gallery);
            return images.AsEnumerable();
        }

        // GET api/Images/5
        public Image GetImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return image;
        }

        // PUT api/Images/5
        public HttpResponseMessage PutImage(int id, Image image)
        {
            if (ModelState.IsValid && id == image.ImageId)
            {
                db.Entry(image).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Images
        public HttpResponseMessage PostImage(Image image)
        {
            if (ModelState.IsValid)
            {
                db.Images.Add(image);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, image);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = image.ImageId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Images/5
        public HttpResponseMessage DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Images.Remove(image);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, image);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}