using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels.Users
{
    public class LoginVM
    {
        [Required(ErrorMessage = "You have to fill in this section.")]
        public string UsernameOrEmail { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool IsRemembered { get; set; }
    }
}
