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
using System.Web.Http.Cors;

namespace JohnnyBravoGalleryApp.Controllers
{
    public class UsersController : ApiController
    {
        private readonly AllRepositories repos;

        public UsersController(AllRepositories repos)
        {
            this.repos = repos;
        }

        public HttpResponseMessage PostUser([FromBody]User user)
        {
            int userId;

            var theUser = repos.UserRepo.GetAll().FirstOrDefault(u => u.Username == user.Username);

            if (theUser == null)
            {
                repos.UserRepo.Add(user);
                userId = user.UserId;
            }
            else
            {
                userId = theUser.UserId;
            }

            if (ModelState.IsValid)
            {
                User userEntity = this.repos.UserRepo.Get(userId);

                UserModel userModel = UserModel.CreateUserModelFromEntity(userEntity);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, userModel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = userModel.UserId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}