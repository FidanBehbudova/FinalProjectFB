using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class UpdateCompanyVM
    {
        public IFormFile? Photo { get; set; }
        
        public ICollection<int> CityIds { get; set; }
        public ICollection<City>? Cities { get; set; }
        public ICollection<Image>? Images { get; set; }
   
        public bool? IsDeleted { get; set; }
       

        public string? Name { get; set; }
        
        public string? Description { get; set; }
      
        public string? FacebookLink { get; set; }
     
        public string? InstagramLink { get; set; }
        
        public string? WebsiteLink { get; set; }
      
        public string? TwitterLink { get; set; }
      
        public string? GmailLink { get; set; }
    }
}
