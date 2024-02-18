using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels.Users
{
    public class ForgotPasswordVM
    {
        [Required]
        [MaxLength(255)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
