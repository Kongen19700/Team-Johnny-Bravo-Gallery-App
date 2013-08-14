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
    public class UsersController : ApiController
    {
        private readonly AllRepositories repos;

        public UsersController(AllRepositories repos)
        {
            this.repos = repos;
        }

        private GalleryEntities db = new GalleryEntities();

        // GET api/Users
        public IEnumerable<UserFullModel> GetUsers()
        {
            var users = db.Users.Include(u => u.Gallery).AsEnumerable();
            return users.Select(x => new UserFullModel()
            {
                UserId = x.UserId,
                Username = x.Username,
            });
        }

        // GET api/Users/5
        public User GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user;
        }

        // PUT api/Users/5
        public HttpResponseMessage PutUser(int id, User user)
        {
            if (ModelState.IsValid && id == user.UserId)
            {
                db.Entry(user).State = EntityState.Modified;

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

        // POST api/Users
        public HttpResponseMessage PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Users/5
        public HttpResponseMessage DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.Remove(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public UserFullModel CreateFullUserModelFromEntity(User entity)
        {
            return new UserFullModel()
            {
                UserId = entity.UserId,
                Username = entity.Username,
                Comments = entity.Comments.Select(c => new CommentModel()
                {
                    CommentId = c.CommentId,
                    ImageId = c.ImageId,
                    Text = c.Text,
                    UserId = c.UserId,
                }),
                Gallery = new GalleryModel()
                {
                    GalleryId = entity.Gallery.GalleryId,
                    Title = entity.Gallery.Title,
                },
            };
        }
    }
}