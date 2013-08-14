using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T item);
        void Delete(int id);
        IQueryable<T> GetAll();
        T Get(int id);
        void Update(int id, T item);
    }
}
