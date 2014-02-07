using Coinage.Domain.Concrete.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Coinage.Data.EntityFramework.Configuration
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("Customer");

            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("CustomerId");

            Property(p => p.Email).HasMaxLength(1000);

            Property(p => p.ModifiedOn).HasColumnType("datetime2");

            #region Navigation Properties

            HasMany(p => p.Roles).WithMany().Map(m => m.ToTable("Customer_CustomerRole"));

            #endregion
        }
    }
}
