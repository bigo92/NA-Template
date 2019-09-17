using Microsoft.EntityFrameworkCore;
using Na.DataAcess.Bases;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NA.DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        (DbContext db, string query) Delete();
        (DbContext db, string query) Select(string query = null); 
        (DbContext db, string query) Update(string query);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _entities;
        private readonly NATemplateContext _context;
        private readonly UnitOfWork<DbContext> _unit;

        public Repository(
            NATemplateContext context,
            UnitOfWork<DbContext> unit)
        {
            _context = context;
            _unit = unit;
            _entities = _context.Set<T>();
        }

        public virtual (DbContext db, string query) Select(string query = null)
        {
            return _context.Select(query).From<T>();
        }

        public virtual void Add(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                _entities.Add(entity);
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
        public void AddRange(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                _context.Set<T>().AddRange(entities);
                _context.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }

        public virtual (DbContext db, string query) Update(string query)
        {
            var newQuery = _context.Set(query).From<T>();
            var idQuery = Guid.NewGuid();
            _unit.lstQuery.Add(new UQuery {
                id = idQuery,
                type = (int) UType.Update,
                query = newQuery.query
            });
            return (_context, _unit.lstQuery.Find(x=>x.id == idQuery).query);
        }

        public virtual (DbContext db, string query) Delete()
        {
            var newQuery = _context.Delete().From<T>();
            var idQuery = Guid.NewGuid();
            _unit.lstQuery.Add(new UQuery
            {
                id = idQuery,
                type = (int)UType.Delete,
                query = newQuery.query
            });
            return (_context, _unit.lstQuery.Find(x => x.id == idQuery).query);
        }
    }
}
