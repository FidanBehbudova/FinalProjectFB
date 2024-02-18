using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class CompanyCity:BaseEntity
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
