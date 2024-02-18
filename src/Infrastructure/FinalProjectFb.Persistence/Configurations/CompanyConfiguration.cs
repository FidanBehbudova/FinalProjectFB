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
    internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.TwitterLink).IsRequired();
            builder.Property(x => x.FacebookLink).IsRequired();
            builder.Property(x => x.GmailLink).IsRequired();
            builder.Property(x => x.WebsiteLink).IsRequired();
            builder.Property(x => x.InstagramLink).IsRequired();



        }
    }
}
