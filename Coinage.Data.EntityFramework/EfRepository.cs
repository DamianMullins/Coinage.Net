using Coinage.Data.EntityFramework.Context;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Coinage.Domain.Data;
using Coinage.Domain.Entites;

namespace Coinage.Data.EntityFramework
{
    public class EfRepository<T> : IRepository<T> 
        where T : Entity
    {
        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        private IDbSet<T> Entities
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

        public T GetById(object id)
        {
            return Entities.Find(id);
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
