using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Abstract.Services
{
    public interface IEditableService<in T> where T : EditableEntity
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
