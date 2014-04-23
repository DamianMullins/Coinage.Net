using Coinage.Domain.Entites;
using Coinage.Domain.Models;

namespace Coinage.Domain.Services
{
    public interface IEditableService<in T>
        where T : EditableEntity
    {
        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>EntityActionResponse object.</returns>
        EntityActionResponse Update(T entity);

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>EntityActionResponse object.</returns>
        EntityActionResponse Create(T entity);
    }
}
