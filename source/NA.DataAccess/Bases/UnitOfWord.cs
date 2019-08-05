using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NA.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.DataAccess.Bases
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        Repository<T> Repository<T>() where T : class;
    }

    public class UnitOfWork<TContext> : IUnitOfWork where TContext: DbContext
    {
        private readonly TContext _context;
        private bool _disposed;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public Repository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<T>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)_repositories[type];
        }

        public void Commit()
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }
            }
            _disposed = true;
        }

        public void Rollback()
        {
            
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
    }
}
