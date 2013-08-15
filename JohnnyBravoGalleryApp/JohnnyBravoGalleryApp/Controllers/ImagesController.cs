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
using DropNet;
using System.Web.Mvc;
using JohnnyBravoGalleryApp.DependencyResolver;
using System.Web.Http.Cors;

namespace JohnnyBravoGalleryApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly AllRepositories repos;

        public ImagesController()
        {
            this.repos = DbDependencyResolver.GetAllRepositories();
        }

        public ImagesController(AllRepositories repos)
        {
            this.repos = repos;
        }

        public HttpResponseMessage PostImage(ImageUploadModel imageUploadModel)
        {
            // Validation
            if (imageUploadModel.AlbumId == 0)
            {
                HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.PartialContent);
                errorResponse.Content = new StringContent("Image album is mandatory");
                return errorResponse;
                throw new HttpResponseException(errorResponse);
            }

            if (imageUploadModel.File == null)
            {
                HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.PartialContent);
                errorResponse.Content = new StringContent("No file provided");
                return errorResponse;
                throw new HttpResponseException(errorResponse);
            }

            if (imageUploadModel.Title == null || imageUploadModel.Title == string.Empty)
            {
                HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.PartialContent);
                errorResponse.Content = new StringContent("Image title is mandatory");
                return errorResponse;
                throw new HttpResponseException(errorResponse);
            }

            if (this.repos.ImageRepo.GetAll().Any(img => img.Title == imageUploadModel.Title && img.AlbumId == imageUploadModel.AlbumId))
            {
                HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.Conflict);
                errorResponse.Content = new StringContent("Image title exists in the current album");
                return errorResponse;
                throw new HttpResponseException(errorResponse);
            }

            if (ModelState.IsValid)
            {
                Album album = this.repos.AlbumRepo.Get(imageUploadModel.AlbumId);

                if (album == null)
                {
                    HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                    errorResponse.Content = new StringContent("No album with such id exists");
                    return errorResponse;
                    throw new HttpResponseException(errorResponse);
                }
                
                imageUploadModel.Url = UploadImage(album.User.Username, imageUploadModel.File);

                Image image = new Image
                {
                    Title = imageUploadModel.Title,
                    Url = imageUploadModel.Url,
                    AlbumId = imageUploadModel.AlbumId
                };

                this.repos.ImageRepo.Add(image);

                Image imageEntity = this.repos.ImageRepo.Get(image.ImageId);

                ImageModel imageModel = ImageFullModel.CreateFullImageModelFromEntity(imageEntity);
                
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Image imported successfully");
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = imageModel.ImageId }));
                 return response;
            }
            else
            {
                HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent("Bad request");
                return errorResponse;
            }
        }

        private string UploadImage(string username, HttpPostedFileBase file)
        {
            const string apiKey = "l4nimqeeebndlvk";
            const string appSecret = "hc00vzx5jhh5gsy";

            const string clientKey = "w6efv2ke7212uc6c";
            const string clientSecret = "bpucboq34ndlxos";

            DropNetClient client = new DropNetClient(apiKey, appSecret, clientKey, clientSecret);

            string path = "/images/user_" + username.ToLower();
            string filename = "img_" + Guid.NewGuid() + file.FileName.Substring(file.FileName.LastIndexOf('.')); 
            DropNet.Models.MetaData metaData =  
                client.UploadFile(path, filename, file.InputStream);
            DropNet.Models.ShareResponse response = client.GetMedia(metaData.Path);

            return response.Url;
        }
    }
}