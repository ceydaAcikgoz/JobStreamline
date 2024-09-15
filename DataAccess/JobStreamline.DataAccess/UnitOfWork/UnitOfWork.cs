using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace JobStreamline.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        public bool IsDisposed { get; private set; } = false;
        private readonly JobStreamlineDbContext _context;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(JobStreamlineDbContext dbContext)
        {
            _context = dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);  
            IsDisposed = true;
        }
    }
}