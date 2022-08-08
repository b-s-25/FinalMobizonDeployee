
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IGenericRepositoryOperation<T>
    {
        IEnumerable<T> GetAll();
        Task<IQueryable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(int Id);
        void Save();
        
        Task<T> GetById(int id, params Expression<Func<T, object>>[] includes);

    }
}
