using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Na.DataAcess.Bases;
using NA.DataAccess.Models;
using NA.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NA.DataAccess.Bases
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChange();
        IRepository<T> Repository<T>() where T : class;

        (DbContext db, string query) Select(string query = null);
    }

    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private bool _disposed;
        public readonly TContext db;
        private readonly IServiceProvider _service;
        public List<UQuery> lstQuery = new List<UQuery>();

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

        public async Task<int> SaveChange()
        {
            var result = 0;
            try
            {
                result = db.SaveChanges();
                if (lstQuery.Count > 0)
                {
                    foreach (var query in lstQuery)
                    {
                        switch (query.type)
                        {
                            case (int)UType.Update:
                                result += await (db, query.query).ExecuteNonQuery();
                                break;
                            case (int)UType.Delete:
                                result += await (db, query.query).ExecuteNonQuery();
                                break;
                        }
                    }
                }
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
            return result;
        }

        public (DbContext, string) Select(string select = null)
        {
            return db.Select(select);
        }
    }

    public class UQuery
    {
        public Guid id { get; set; }

        public string query { get; set; }

        public int type { get; set; }
    }

    public enum UType
    {
        Add,
        Update,
        Delete
    }
}
