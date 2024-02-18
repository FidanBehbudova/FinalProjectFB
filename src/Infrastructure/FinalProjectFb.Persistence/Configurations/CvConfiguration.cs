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


	internal class CvConfiguration : IEntityTypeConfiguration<Cv>
	{
		public void Configure(EntityTypeBuilder<Cv> builder)
		{
			builder.Property(x => x.Name).IsRequired().HasMaxLength(25);
			builder.HasIndex(x => x.Name).IsUnique();

			builder.Property(x => x.Surname).IsRequired().HasMaxLength(25);
			builder.HasIndex(x => x.Surname).IsUnique();

			builder.Property(x => x.Address).IsRequired().HasMaxLength(100);
			builder.HasIndex(x => x.Address).IsUnique();

			builder.Property(x => x.FatherName).IsRequired().HasMaxLength(25);
			builder.HasIndex(x => x.FatherName).IsUnique();

			builder.Property(x => x.FinnishCode).IsRequired().HasMaxLength(7);
			builder.HasIndex(x => x.FinnishCode).IsUnique();
		}
	}
}
