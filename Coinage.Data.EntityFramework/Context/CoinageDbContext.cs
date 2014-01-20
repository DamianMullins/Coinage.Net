using Coinage.Domain.Concrete.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace Coinage.Data.EntityFramework.Context
{
    public class CoinageDbContext : DbContext, IDbContext
    {
        public CoinageDbContext() 
            : base("Coinage")
        {
            //((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 5000;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                                    .Where(type => !string.IsNullOrEmpty(type.Namespace) &&
                                                    type.BaseType != null &&
                                                    type.BaseType.IsGenericType &&
                                                    type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : EditableEntity
        {
            return base.Entry(entity);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : EditableEntity
        {
            return base.Set<TEntity>();
        }
    }
}
