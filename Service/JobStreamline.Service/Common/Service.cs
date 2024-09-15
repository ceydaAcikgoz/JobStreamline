using System.Collections;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using JobStreamline.DataAccess;
using AutoMapper;

namespace JobStreamline.Service
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IRepository<TEntity> repository;
        protected readonly IMapper _iMapper;

        public Service(IMapper Mapper, IUnitOfWork UnitOfWork)
        {
            this.unitOfWork = UnitOfWork;
            repository = this.unitOfWork.GetRepository<TEntity>();
            _iMapper = Mapper;
        }

        public virtual void Create(TEntity Entity)
        {
            try
            {
                repository.Add(Entity);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual void Create(List<TEntity> Entitys)
        {
            try
            {
                repository.Add(Entitys);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual void Update(TEntity Entity)
        {
            try
            {
                repository.Update(Entity);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual void Update(List<TEntity> Entitys)
        {
            try
            {
                repository.Update(Entitys);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual void Delete(List<TEntity> Entitys)
        {
            try
            {
                repository.Delete(Entitys);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }
        public virtual void Delete(TEntity Entity)
        {
            try
            {
                var id = (Guid)Entity.GetType()?.GetProperty("Id")?.GetValue(Entity);
                repository.Delete(id);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual void Delete(Guid id)
        {
            try
            {
                repository.Delete(id);
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual TEntity? Get(Guid id)
        {
            try
            {
                var item = repository.GetById(id);
                return item;
            }
            catch (System.Exception Ex)
            {
                throw (Ex);
            }
        }

        public virtual List<TEntity> GetAll()
        {
            return repository.GetAll().ToList();
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return repository.GetAll(predicate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);  
        }
    }
}