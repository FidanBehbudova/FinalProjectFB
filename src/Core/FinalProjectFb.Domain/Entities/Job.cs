using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class Job:BaseNameableEntity
    {
        

        public List<Image>? Images { get; set; }
        [ForeignKey("CompanyId")]
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

		public List<Cv>? Cvs { get; set; }
		public DateTime Deadline { get; set; }

        
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }


		
		public string Requirement { get; set; }

		public decimal Salary { get; set; }
        public string JobNature { get; set; }
        public string Experience { get; set; }

        public string Vacancy { get; set; }
      
       





    }
}
