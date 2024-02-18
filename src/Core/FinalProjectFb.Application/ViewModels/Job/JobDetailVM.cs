using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class JobDetailVM
    {
        public List<Job> RelatedJobs { get; set; }
        public List<Job> CompanyJobs { get; set; }
        public int JobId { get; set; }

        //public Job Jobs { get; set; }
        public JobItemVM Job { get; set; }

        //public string Image  { get; set; }
    }
}
