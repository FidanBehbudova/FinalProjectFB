using FinalProjectFb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class JobItemVM
    {
        public List<Image>? Images { get; set; }
        public int JobId { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public List<CompanyCity> CompanyCities { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }

		
		public string Requirement { get; set; }

		public decimal Salary { get; set; }
        public string JobNature { get; set; }
        public string Experience { get; set; }

        public string Vacancy { get; set; }
       
        public DateTime DiscontinuationDate { get; set; }

    }
}
