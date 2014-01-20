using Coinage.Data.EntityFramework.Context;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Data;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Coinage.Data.EntityFramework
{
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        private readonly IDbContext _context;
        private IDbSet<T> _entities;

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
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                var fail = new Exception("", dbException);
                throw fail;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities.Attach(entity);

                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                var fail = new Exception("", dbException);
                throw fail;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                var fail = new Exception("", dbException);
                throw fail;
            }
        }

        public IQueryable<T> Table
        {
            get { return Entities; }
        }

        private IDbSet<T> Entities
        {
            get { return _entities ?? (_entities = _context.Set<T>()); }
        }
    }
}
