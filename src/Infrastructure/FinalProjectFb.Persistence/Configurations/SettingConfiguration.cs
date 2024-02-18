using FinalProjectFb.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Configurations
{
    internal class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(x => x.Key).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Key).IsUnique();

            builder.Property(x => x.Value).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Value).IsUnique();



        }
    }
}
