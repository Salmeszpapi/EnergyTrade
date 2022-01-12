using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SSM.Common.Domain.EntityConfigurations {
    public class BrandConfiguration : EntityTypeConfiguration<Brand> {
        public BrandConfiguration() {
            ToTable("Brand");

            HasKey<long>(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("BrandId");
        }
    }
}