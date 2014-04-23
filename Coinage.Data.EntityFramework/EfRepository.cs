using Coinage.Data.EntityFramework.Context;
using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Coinage.Data.EntityFramework
{
    public class EfRepository<T> : IRepository<T>
        where T : Entity
    {
        protected readonly IDbContext _context;
        protected IDbSet<T> _entities;
        protected IDbSet<T> Entities
        {
            get { return _entities ?? (_entities = _context.Set<T>()); }
        }

        public IQueryable<T> Table
        {
            get { return Entities; }
        }

        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return Entities.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Entities.Count(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            // Careful with this one
            return Entities.AsEnumerable();
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate).AsEnumerable();
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                throw new Exception("Insert failed", dbException);
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    Entities.Attach(entity);
                    entry.State = EntityState.Modified;
                }

                if (entity is EditableEntity)
                {
                    (entity as EditableEntity).ModifiedOn = DateTime.Now;
                }

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                throw new Exception("Update failed", dbException);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                throw new Exception("Delete failed", dbException);
            }
        }
    }
}
