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

        public HttpResponseMessage PostAlbum([FromBody]Album album)
        {
            //TODO: validation

            this.repos.AlbumRepo.Add(album);

            Album entityAlbum = this.repos.AlbumRepo.Get(album.AlbumId);

            AlbumModel albumModel = AlbumFullModel.CreateFullAlbumModelFromEntity(entityAlbum);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, albumModel);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = albumModel.AlbumId }));
            return response;
        }
    }
}