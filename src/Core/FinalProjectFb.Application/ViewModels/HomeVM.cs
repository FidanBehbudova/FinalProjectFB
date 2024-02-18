using FinalProjectFb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class HomeVM
    {
        public ICollection<Category> Categories { get; set; }
        public ICollection<Job> Jobs { get; set; }
       
        public ICollection<Company> Companies { get; set; }

    }
}
