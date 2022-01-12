using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
namespace SSM.Common.Domain.EntityConfigurations {
    public class StockItemConfiguration : EntityTypeConfiguration<StockItem> {
        public StockItemConfiguration() {
            ToTable("StockItem");

            HasKey<long>(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("StockItemId");
        }
    }
}
