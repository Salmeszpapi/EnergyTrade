using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SSM.Common.Domain.EntityConfigurations {
    public class UserConfiguration : EntityTypeConfiguration<User> {
        public UserConfiguration() {
            ToTable("User");

            HasKey<long>(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("UserId");

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
