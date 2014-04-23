using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using System.Threading.Tasks;

namespace Coinage.Domain.Services
{
    public interface IEditableServiceAsync<in T> : IEditableService<T> 
        where T : EditableEntity
    {
        /// <summary>
        /// Asynchronously update an entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>EntityActionResponse object.</returns>
        Task<EntityActionResponse> UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously create an entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>EntityActionResponse object.</returns>
        Task<EntityActionResponse> CreateAsync(T entity);
    }
}
