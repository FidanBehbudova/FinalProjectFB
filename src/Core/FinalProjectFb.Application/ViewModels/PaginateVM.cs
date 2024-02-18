using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class PaginateVM<T> where T : class, new()
    {
        public ICollection<T> Items { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int CompanyId { get; set; }
        public int JobId { get; set; }

    }
}
