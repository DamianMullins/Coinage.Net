using Coinage.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coinage.Domain.Data
{
    public interface IRepositoryAsync<T> : IRepository<T>
        where T : Entity
    {
        /// <summary>
        /// Asynchronously count all entities of a certain type.
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// Asynchronously count all entities matching an expression of a certain type.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Asynchronously get all entities of a certain type.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Asynchronously find a single entity matching an expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Asynchronously find all entities matching an expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
    }
}
