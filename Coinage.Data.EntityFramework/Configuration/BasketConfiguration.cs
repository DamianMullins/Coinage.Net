using Coinage.Domain.Entites;
using System.Data.Entity.ModelConfiguration;

namespace Coinage.Data.EntityFramework.Configuration
{
    public class BasketConfiguration : EntityTypeConfiguration<Basket>
    {
        public BasketConfiguration()
        {
            ToTable("Basket");

            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("BasketId");

            Property(p => p.ModifiedOn).HasColumnType("datetime2");
        }
    }
}
