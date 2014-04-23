using System.Threading.Tasks;
using Coinage.Domain.Entites;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Coinage.Data.EntityFramework.Context
{
    public interface IDbContext
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : Entity;

        IDbSet<TEntity> Set<TEntity>() where TEntity : Entity;

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
