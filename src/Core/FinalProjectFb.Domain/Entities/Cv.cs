using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
	public class Cv:BaseNameableEntity
	{
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string Surname { get; set; }
		public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
		public string EmailAddress { get; set; }
		public string Address { get; set; }
        public string Birthday { get; set; }
		public string FinnishCode { get; set; }
		public Job? Job { get; set; }
		public int? JobId { get; set; }

	}
}
