using System;

namespace CommandRecognition.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
    }
}