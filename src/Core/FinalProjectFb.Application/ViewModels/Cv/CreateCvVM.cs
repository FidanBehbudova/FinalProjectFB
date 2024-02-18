using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels.Cv
{
	public class CreateCvVM
	{
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string FatherName { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string Birthday { get; set; }
		[MinLength(7)]
		[MaxLength(7)]
		public string FinnishCode { get; set; }
		public int JobId { get; set; }

	}
}
