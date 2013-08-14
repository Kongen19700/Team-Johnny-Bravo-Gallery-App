using Gallery.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected IDbSet<T> DbSet { get; set; }
        protected DbContext Context { get; set; }

        public EfRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        public void Add(T item)
        {
            ReinitializeContext();

            DbEntityEntry entry = this.Context.Entry(item);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(item);
            }

            this.Context.SaveChanges();
        }

        private void ReinitializeContext()
        {
            this.Context = (DbContext) Activator.CreateInstance(this.Context.GetType(), null);
            this.DbSet = this.Context.Set<T>();
        }

        public virtual void Delete(T entity)
        {
            ReinitializeContext();

            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }

            this.Context.SaveChanges();
        }

        public void Delete(int id)
        {
            ReinitializeContext();

            var entity = this.Get(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public IQueryable<T> GetAll()
        {
            ReinitializeContext();

            return this.DbSet;
        }

        public T Get(int id)
        {
            ReinitializeContext();

            return this.DbSet.Find(id);
        }

        public void Update(int id, T item)
        {
            ReinitializeContext();

            DbEntityEntry entry = this.Context.Entry(item);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(item);
            }

            entry.State = EntityState.Modified;

            this.Context.SaveChanges();
        }
    }
}
