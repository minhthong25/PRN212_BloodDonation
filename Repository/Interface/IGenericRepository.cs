using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Repository.Models;

namespace Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        T? GetById(object id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T? FirstOrDefault(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        void SaveChanges();
        void Dispose();
        void Save();
    }
}