using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class CreateCityVM
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
