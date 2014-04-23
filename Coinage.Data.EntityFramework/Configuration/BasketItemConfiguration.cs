using Coinage.Domain.Entites;
using System.Data.Entity.ModelConfiguration;

namespace Coinage.Data.EntityFramework.Configuration
{
    public class BasketItemConfiguration : EntityTypeConfiguration<BasketItem>
    {
        public BasketItemConfiguration()
        {
            ToTable("BasketItem");

            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("BasketItemId");

            Property(p => p.ModifiedOn).HasColumnType("datetime2");

            HasRequired(bi => bi.Basket)
                .WithMany(b => b.BasketItems)
                .HasForeignKey(bi => bi.BasketId);

            HasRequired(bi => bi.Product)
                .WithMany()
                .HasForeignKey(bi => bi.ProductId);
        }
    }
}
