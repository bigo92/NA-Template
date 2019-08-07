using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NA.DataAccess.Models;
using NA.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace NA.DataAccess.Bases
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IRepository<T> Repository<T>() where T : class;

        TransactionScope BeginTransaction(Action<TransactionOptions> options = null);
    }

    public class UnitOfWork<TContext> : IUnitOfWork where TContext: DbContext
    {
        private bool _disposed;
        private readonly TContext _context;
        private readonly IServiceProvider _service;

        public UnitOfWork(IServiceProvider service, TContext context)
        {
            _service = service;
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return _service.GetService<IRepository<T>>();
        }

        public TransactionScope BeginTransaction(Action<TransactionOptions> options = null)
        {
            // option default
            var option = new TransactionOptions {
                IsolationLevel = IsolationLevel.RepeatableRead
            };
            if (options != null)
            {
                options.Invoke(option);
            }
            return new TransactionScope(TransactionScopeOption.Required, option, TransactionScopeAsyncFlowOption.Enabled);
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
