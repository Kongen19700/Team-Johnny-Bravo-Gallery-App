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

        [HttpGet]
        public AlbumFullModel GetUserGallery(int userId)
        {
            User user = this.repos.UserRepo.Get(userId);

            Album gallery = this.repos.AlbumRepo.GetAll()
                .Where(a => a.UserId == user.UserId && a.ParentAlbumId == null)
                .FirstOrDefault();

            if (gallery == null)
            {
                gallery = new Album()
                {
                    Title = "Gallery of " + user.Username,
                    User = user,
                    ParentAlbumId = null,
                };

                this.repos.AlbumRepo.Add(gallery);
            }

            AlbumFullModel galleryModel = AlbumFullModel.CreateFullAlbumModelFromEntity(gallery);

            return galleryModel;
        }

        [HttpGet]
        public IEnumerable<AlbumModel> GetAllGalleries()
        {
            var galleriesEntities = this.repos.AlbumRepo.GetAll()
                .Where(a => a.ParentAlbumId == null || a.ParentAlbumId == 0);

            if (galleriesEntities == null)
            {
                var errorResponse = this.Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, "Items not found");
                throw new HttpResponseException(errorResponse);
            }

            var galleriesModels = galleriesEntities.ToList().
                Select(a => new AlbumModel()
                {
                    AlbumId = a.AlbumId,
                    Title = a.Title,
                });

            return galleriesModels;
        }
    }
}