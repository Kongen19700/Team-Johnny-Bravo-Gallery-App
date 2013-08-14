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

        private GalleryEntities db = new GalleryEntities();

        // GET api/Comments
        public IEnumerable<Comment> GetComments()
        {
            var comments = db.Comments.Include(c => c.Image).Include(c => c.User);
            return comments.AsEnumerable();
        }

        // GET api/Comments/5
        public Comment GetComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return comment;
        }

        // PUT api/Comments/5
        public HttpResponseMessage PutComment(int id, Comment comment)
        {
            if (ModelState.IsValid && id == comment.CommentId)
            {
                db.Entry(comment).State = EntityState.Modified;

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

        // POST api/Comments
        public HttpResponseMessage PostComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, comment);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = comment.CommentId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Comments/5
        public HttpResponseMessage DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Comments.Remove(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, comment);
        }
    }
}