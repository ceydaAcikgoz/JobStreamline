using System;
using System.Linq.Expressions;

namespace JobStreamline.DataAccess;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(Expression<Func<T, bool>> predicate); 
    T GetById(Guid id);
    T Get(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Add(List<T> entitys);
    void Update(T entity);
    void Update(List<T> entitys);
    void Delete(T entity);
    void Delete(Guid id);
    void Delete(List<T> entitys);
}
