using Microsoft.EntityFrameworkCore;
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
        IEnumerable<T> Get();
        //IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> Table { get; }
        IEnumerable<T> GetAll();
        T GetById(object id);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _entities;
        private NATemplateContext _context { get; set; }

        public Repository(NATemplateContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public virtual IQueryable<T> Table
        {
            get { return _entities; }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public virtual T GetById(object id)
        {
            return _entities.Find(id);
        }

        public virtual void Insert(T entity)
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
        public void BulkInsert(IEnumerable<T> entities)
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
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                SetEntryModified(entity);
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                _entities.Remove(entity);
            }
            catch (Exception dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
        public virtual void SetEntryModified(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> Get()
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
