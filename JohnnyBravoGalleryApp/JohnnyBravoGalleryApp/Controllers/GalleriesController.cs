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
    public class GalleriesController : ApiController
    {

        private readonly AllRepositories repos;

        public GalleriesController(AllRepositories repos)
        {
            this.repos = repos;
        }

        private GalleryEntities db = new GalleryEntities();

        // GET api/Galleries
        public IEnumerable<Gallery.Data.Gallery> GetGalleries()
        {
            var galleries = db.Galleries.Include(g => g.User);
            return galleries.AsEnumerable();
        }

        // GET api/Galleries/5
        public Gallery.Data.Gallery GetGallery(int id)
        {
            Gallery.Data.Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return gallery;
        }

        // PUT api/Galleries/5
        public HttpResponseMessage PutGallery(int id, Gallery.Data.Gallery gallery)
        {
            if (ModelState.IsValid && id == gallery.GalleryId)
            {
                db.Entry(gallery).State = EntityState.Modified;

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

        // POST api/Galleries
        public HttpResponseMessage PostGallery(Gallery.Data.Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Galleries.Add(gallery);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, gallery);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = gallery.GalleryId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Galleries/5
        public HttpResponseMessage DeleteGallery(int id)
        {
            Gallery.Data.Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Galleries.Remove(gallery);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, gallery);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public GalleryFullModel CreateFullGalleryModelFromEntity(Gallery.Data.Gallery entity)
        {
            return new GalleryFullModel()
            {
                GalleryId = entity.GalleryId,
                Title = entity.Title,
                Albums = entity.Albums.Select(a => new AlbumModel()
                {
                    AlbumId = a.AlbumId,
                    GalleryId = a.GalleryId,
                    ParentAlbumId = a.ParentAlbumId,
                    Title = a.Title,
                }),
                Images = entity.Images.Select( i => new ImageModel()
                {
                    AlbumId = i.AlbumId,
                    GalleryId = i.GalleryId,
                    ImageId = i.ImageId,
                    Title = i.Title,
                    Url = i.Url,
                }),
                User =  new UserModel() {
                    UserId = entity.User.UserId,
                    Username = entity.User.Username,
                },
            };
        }
    }
}