using System.Data;

namespace JobStreamline.DataAccess;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    bool IsDisposed { get; }
}