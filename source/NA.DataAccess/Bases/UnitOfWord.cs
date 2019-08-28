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
    }

    public class UnitOfWork<TContext> : IUnitOfWork where TContext: DbContext
    {
        private bool _disposed;
        public readonly TContext db;
        private readonly IServiceProvider _service;

        public UnitOfWork(IServiceProvider service, TContext context)
        {
            _service = service;
            db = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return _service.GetService<IRepository<T>>();
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

        public void Save()
        {
            try
            {
                db.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
    }
}
