using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectFb.ViewModels
{
    public class CreateJobVM
    {
       
     
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        
		public string Requirement { get; set; }

		public int CompanyId { get; set; }

  
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        



        public DateTime Deadline { get; set; }

        
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

     
        public string JobNature { get; set; }

       
        public string Experience { get; set; }

     
        public string Vacancy { get; set; }

    
    

       
        
    }
}
