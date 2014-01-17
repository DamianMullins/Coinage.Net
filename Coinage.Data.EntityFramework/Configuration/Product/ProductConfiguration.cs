using System.Data.Entity.ModelConfiguration;

namespace Coinage.Data.EntityFramework.Configuration.Product
{
    public class ProductConfiguration : EntityTypeConfiguration<Domain.Concrete.Entities.Product>
    {
        public ProductConfiguration()
        {
            ToTable("Product");

            HasKey(p => p.ProductId);

            Property(p => p.Name).IsRequired().HasMaxLength(500);
        }
    }
}
