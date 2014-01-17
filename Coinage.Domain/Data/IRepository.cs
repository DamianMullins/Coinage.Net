using System.Linq;

namespace Coinage.Domain.Data
{
    public interface IRepository<T>
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
