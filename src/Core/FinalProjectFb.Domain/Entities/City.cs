using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class City:BaseNameableEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<CompanyCity> CompanyCities { get; set; }
    }

}
