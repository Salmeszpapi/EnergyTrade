using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SSM.Common.Domain.EntityConfigurations {
    public class ProductConfiguration : EntityTypeConfiguration<Product> {
        public ProductConfiguration() {
            ToTable("Product");

            HasKey<long>(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("ProductId");
        }
    }
}