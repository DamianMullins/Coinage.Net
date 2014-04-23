using Coinage.Domain.Entites;
using System.Data.Entity.ModelConfiguration;

namespace Coinage.Data.EntityFramework.Configuration
{
    public class CustomerRoleConfiguration : EntityTypeConfiguration<CustomerRole>
    {
        public CustomerRoleConfiguration()
        {
            ToTable("CustomerRole");

            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("CustomerRoleId");

            Property(p => p.Name).IsRequired().HasMaxLength(255);
        }
    }
}
