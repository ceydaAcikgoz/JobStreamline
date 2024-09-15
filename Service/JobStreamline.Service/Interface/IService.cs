using System.Linq.Expressions;

namespace JobStreamline.Service
{
    public interface IService<TEntity>:IDisposable
    {
        void Create(TEntity Entity);
        void Create(List<TEntity> Entitys);
        void Update(TEntity Entity);
        void Update(List<TEntity> Entitys);
        TEntity? Get(Guid id);
        void Delete(Guid id);
        void Delete(TEntity Entity);
        void Delete(List<TEntity> Entitys);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
    }
}