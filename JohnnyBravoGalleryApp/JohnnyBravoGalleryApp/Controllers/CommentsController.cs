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
using JohnnyBravoGalleryApp.Pubnub;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Web.Script;
using System.Web.Script.Serialization;

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
            // Validation
            if (comment.UserId == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "User is mandatory");
            }

            if (comment.ImageId == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "Image is mandatory");
            }

            if ((comment.Text == null || comment.Text == string.Empty))
            {
                return Request.CreateErrorResponse(HttpStatusCode.PartialContent, "Comment text is mandatory");
            }

            if (ModelState.IsValid)
            {
                this.repos.CommentRepo.Add(comment);
                var commentEntity = this.repos.CommentRepo.Get(comment.CommentId);

                CommentModel commentModel = CommentModel.CreateCommentModelFromEntity(commentEntity);

                CommentNotification(commentEntity);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, commentModel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = commentModel.CommentId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }
        }

        private void CommentNotification(Comment comment)
        {
            string username = comment.Image.Album.User.Username.ToLower();
            string message = comment.User.Username + " has commented on image " + comment.Image.Title;
            
            string channel = username + "_notifications";

            PubnubAPI pubnub = new PubnubAPI(
                "pub-c-3f3b9dbe-c842-48bd-a306-938f4d1d36ad",
                "sub-c-eaa6a17c-05c5-11e3-a005-02ee2ddab7fe",
                "sec-c-OWNhZThiZGQtN2FmYS00Mjg5LThiMGItMTNlYTY5NmY1ZGE4",
                true
            );

            pubnub.Publish(channel, message);
        }
    }
}