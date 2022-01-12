using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;
using SSM.Common.Domain.EntityConfigurations;
using Csaba.Entity;

namespace SSM.Common.Services.DataContext {
    public class EnergyContext : DbContext {

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<User> Users { get; set; }


        public EnergyContext() : base("Data Source=localhost;Initial Catalog=Energy;Integrated Security=true;") {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EnergyContext, EnergyTrade.Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ForeignKeyIndexConvention>();

            modelBuilder.Configurations.Add(new BrandConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new StockConfiguration());
            modelBuilder.Configurations.Add(new StockItemConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}
