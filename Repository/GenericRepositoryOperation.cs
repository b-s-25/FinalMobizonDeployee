using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RepositoryLayer
{
    public class GenericRepositoryOperation<T> : IGenericRepositoryOperation<T> where T : class
    {

        private readonly DbContext _Context;
        private readonly DbSet<T> _dbset;
        IQueryable<T> query;
        T entity;
        public GenericRepositoryOperation(ProductDbContext context)
        {
            _Context = context;
            _dbset = _Context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public async Task<IQueryable<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> result = _dbset;
                query = includes.Aggregate(result, (current, includeProperty) => current.Include(includeProperty));
            }
            catch (SqlException ex)
            {

            }
            return query;
        }
        public void Add(T entity)
        {
            try
            {
                _dbset.Add(entity);
                _Context.SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
            _Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
            _Context.SaveChanges();
        }

        public T GetById(int Id)
        {
            return _dbset.Find(Id);
        }

        public void Save()
        {
            try
            {
                _Context.SaveChanges();
            }
            catch(Exception ex)
            {

            }
            
        }
        public async Task<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> result = _dbset;
                query = includes.Aggregate(result, (current, includeProperty) => current.Include(includeProperty));
                entity = _dbset.Find();
                return entity;
            }
            catch (SqlException ex)
            {
                return null;
            }

        }
    }





}
