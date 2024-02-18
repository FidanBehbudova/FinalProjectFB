using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.DAL;
using FinalProjectFb.Persistence.Implementations.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Repositories
{

	public class CvRepository : Repository<Cv>, ICvRepository
	{
		public CvRepository(AppDbContext context) : base(context)
		{
		}
	}
}
