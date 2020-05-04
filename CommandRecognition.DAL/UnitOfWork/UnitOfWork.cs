using CommandRecognition.DAL.Interface;
using CommandRecognition.DAL.Repository;
using System;
using System.Collections;

namespace CommandRecognition.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationContext _context;
        private Hashtable _repositories;

        public UnitOfWork(IApplicationContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();

                foreach (IDisposable repository in _repositories.Values)
                {
                    repository.Dispose();
                }
            }

            _disposed = true;
        }
    }
}