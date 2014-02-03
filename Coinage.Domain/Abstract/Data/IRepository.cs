using Coinage.Domain.Concrete.Entities;
using System.Linq;

namespace Coinage.Domain.Abstract.Data
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
