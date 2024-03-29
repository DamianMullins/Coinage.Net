﻿using System.Data.Entity.ModelConfiguration;
using Coinage.Domain.Entites;

namespace Coinage.Data.EntityFramework.Configuration
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Product");

            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("ProductId");

            Property(p => p.Name).IsRequired().HasMaxLength(500);
            Property(p => p.Price).IsRequired().HasColumnType("Money");

            Property(p => p.ModifiedOn).HasColumnType("datetime2");
        }
    }
}
