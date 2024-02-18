using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class Image:BaseEntity
    {
        public string Url { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public bool? IsPrimary { get; set; }
        public int? JobId { get; set; }
        public Job Job { get; set; }
        
    }
}
