using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SSM.Common.Domain.EntityConfigurations
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            ToTable("Message");

            HasKey<long>(x => x.Id)
                    .Property(x => x.Id)
                    .HasColumnName("MessageId");
        }
    }
}
