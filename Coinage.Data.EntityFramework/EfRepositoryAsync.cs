using Coinage.Data.EntityFramework.Context;
using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coinage.Data.EntityFramework
{
    public class EfRepositoryAsync<T> : EfRepository<T>, IRepositoryAsync<T>
        where T : Entity
    {
        public EfRepositoryAsync(IDbContext context) 
            : base(context)
        {
        }

        public async Task<int> CountAsync()
        {
            return await Entities.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.CountAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Careful with this one
            return await Entities.ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public async Task<int> InsertAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            Entities.Add(entity);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
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

                return await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbException)
            {
                throw new Exception("Update failed", dbException);
            }
        }

        public Task<int> DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
