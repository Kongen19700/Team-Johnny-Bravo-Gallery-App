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
using Gallery.Repositories;
using JohnnyBravoGalleryApp.Models;
using Gallery.Data;

namespace JohnnyBravoGalleryApp.Controllers
{
    public class AlbumsController : ApiController
    {
        private readonly AllRepositories repos;

        public AlbumsController(AllRepositories repos)
        {
            this.repos = repos;
        }

        public AlbumFullModel GetAlbum(int id)
        {
            Album album = this.repos.AlbumRepo.Get(id);

            if (album == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            AlbumFullModel albumModel = AlbumFullModel.CreateFullAlbumModelFromEntity(album);

            return albumModel;
        }

        [HttpPost]
        public HttpResponseMessage PostAlbum([FromBody]Album album)
        {
            // Validation
            if (album.UserId == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "Iser is mandatory");
            }

            if (album.ParentAlbumId == null || album.ParentAlbumId == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "Parent album is mandatory");
            }

            if (album.Title == null || album.Title == string.Empty)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "Album title is mandatory");
            }

            if (this.repos.AlbumRepo.GetAll().Any(a => a.ParentAlbumId == album.ParentAlbumId && a.Title == album.Title))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Album title exists in the current context");
            }

            if (ModelState.IsValid)
            {
                this.repos.AlbumRepo.Add(album);

                Album entityAlbum = this.repos.AlbumRepo.Get(album.AlbumId);

                AlbumModel albumModel = AlbumFullModel.CreateFullAlbumModelFromEntity(entityAlbum);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, albumModel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = albumModel.AlbumId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }
        }
    }
}