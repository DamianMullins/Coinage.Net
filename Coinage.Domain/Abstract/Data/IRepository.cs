using System.Linq;

namespace Coinage.Domain.Abstract.Data
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
