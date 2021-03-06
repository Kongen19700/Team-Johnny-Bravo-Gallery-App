﻿using System;
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
    public class UsersController : ApiController
    {
        private readonly AllRepositories repos;

        public UsersController(AllRepositories repos)
        {
            this.repos = repos;
        }

        [HttpPost]
        public HttpResponseMessage PostUser([FromBody]User user)
        {
            // Validation
            if (user.Username == null || user.Username == string.Empty)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "Username is mandatory");
            }

            var theUser = repos.UserRepo.GetAll().FirstOrDefault(u => u.Username == user.Username);

            int userId;
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }
        }
    }
}