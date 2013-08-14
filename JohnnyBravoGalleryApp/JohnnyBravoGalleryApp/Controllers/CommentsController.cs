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
    public class CommentsController : ApiController
    {
        private readonly AllRepositories repos;

        public CommentsController(AllRepositories repos)
        {
            this.repos = repos;
        }

        // POST api/Comments
        public HttpResponseMessage PostComment(Comment comment)
        {
            //TODO: Validate
            if (ModelState.IsValid)
            {
                var user = this.repos.UserRepo.Get(5);
                comment.User = user;

                var image = this.repos.ImageRepo.Get(comment.ImageId);
                comment.Image = image;

                this.repos.CommentRepo.Add(comment);

                CommentModel commentModel = CommentModel.CreateCommentModelFromEntity(comment);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, commentModel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = commentModel.CommentId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}