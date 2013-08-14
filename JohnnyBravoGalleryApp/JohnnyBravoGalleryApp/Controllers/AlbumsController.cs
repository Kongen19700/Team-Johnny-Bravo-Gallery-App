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
    public class AlbumsController : ApiController
    {
        private readonly AllRepositories repos;

        public AlbumsController(AllRepositories repos)
        {
            this.repos = repos;
        }

        private GalleryEntities db = new GalleryEntities();

        // GET api/Albums
        public IEnumerable<Album> GetAlbums()
        {
            var albums = db.Albums.Include(a => a.Album1).Include(a => a.Gallery);
            return albums.AsEnumerable();
        }

        // GET api/Albums/5
        public Album GetAlbum(int id)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return album;
        }

        // PUT api/Albums/5
        public HttpResponseMessage PutAlbum(int id, Album album)
        {
            if (ModelState.IsValid && id == album.AlbumId)
            {
                db.Entry(album).State = EntityState.Modified;

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

        // POST api/Albums
        public HttpResponseMessage PostAlbum(Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, album);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = album.AlbumId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Albums/5
        public HttpResponseMessage DeleteAlbum(int id)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Albums.Remove(album);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, album);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public AlbumFullModel CreateFullAlbumModelFromEntity(Album entity)
        {
            return new AlbumFullModel()
            {
                AlbumId = entity.AlbumId,
                GalleryId = entity.GalleryId,
                Title = entity.Title,
                ParentAlbumId = entity.ParentAlbumId,
                ParentAlbum = new AlbumModel()
                {
                    AlbumId = entity.Album1.AlbumId,
                    GalleryId = entity.Album1.GalleryId,
                    ParentAlbumId = entity.Album1.ParentAlbumId,
                    Title = entity.Album1.Title
                },
                SubAlbums = entity.Albums1.Select(a => new AlbumModel()
                {
                    AlbumId = a.AlbumId,
                    GalleryId = a.GalleryId,
                    ParentAlbumId = a.ParentAlbumId,
                    Title = a.Title,
                }),
                Images = entity.Images.Select(i => new ImageModel()
                {
                    AlbumId = i.AlbumId,
                    GalleryId = i.GalleryId,
                    ImageId = i.ImageId,
                    Title = i.Title,
                    Url = i.Url,
                }),
                Gallery = new GalleryModel()
                {
                    GalleryId = entity.Gallery.GalleryId,
                    Title = entity.Gallery.Title
                }
            };
        }
    }
}