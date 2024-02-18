using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class UpdateSettingVM
    {
       
        [MaxLength(100)]
        public string? Key { get; set; }
     
        [MaxLength(100)]
        public string? Value { get; set; }
    }
}
