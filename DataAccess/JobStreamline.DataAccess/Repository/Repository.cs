using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobStreamline.DataAccess
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(JobStreamlineDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null.");

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        #region IRepository Members
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).SingleOrDefault();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Add(List<T> entitys)
        {
            _dbSet.AttachRange(entitys);
            _dbSet.AddRange(entitys);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.AttachRange(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }


        public void Update(List<T> entitys)
        {
            _dbSet.AttachRange(entitys);
            foreach (var entity in entitys)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _dbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(List<T> entitys)
        {
            _dbSet.AttachRange(entitys);
            _dbSet.RemoveRange(entitys);
            _dbContext.SaveChanges();
            #endregion
        }
    }
}