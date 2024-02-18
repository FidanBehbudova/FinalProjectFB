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
    internal class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(x => x.Salary).IsRequired().HasColumnType("decimal(6,2)");
           

            builder.Property(c => c.JobNature).IsRequired().HasMaxLength(25);
            builder.Property(x => x.Experience).IsRequired().HasColumnType("text");
            builder.Property(x => x.Requirement).IsRequired().HasColumnType("text");




            builder.Property(c => c.Vacancy).IsRequired().HasMaxLength(25);


        }
    }
}
