using System.Linq;
using Coinage.Domain.Entites;

namespace Coinage.Domain.Data
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Table { get; }

        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
