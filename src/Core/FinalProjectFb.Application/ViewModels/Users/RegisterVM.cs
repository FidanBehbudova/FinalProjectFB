using FinalProjectFb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels.Users
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "You have to fill in this section.")]
        [MinLength(3,ErrorMessage = "The length of the name cannot be shorter than 3 letters.")]
        [MaxLength(25)]
        public string Name { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        [MinLength(10, ErrorMessage = "The length of the name cannot be shorter than 10 letters.")]
        [MaxLength(255)]
        public string Email { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        [MinLength(8, ErrorMessage = "The length of the name cannot be shorter than 8 letters.")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        [MinLength(8, ErrorMessage = "The length of the name cannot be shorter than 8 letters.")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        [MinLength(5, ErrorMessage = "The length of the name cannot be shorter than 5 letters.")]
        [MaxLength(50)]
        public string Username { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        [MinLength(3, ErrorMessage = "The length of the name cannot be shorter than 3 letters.")]
        [MaxLength(50)]
        public string Surname { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        public Gender Gender { get; set; }


        [Required(ErrorMessage = "You have to fill in this section.")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "You have to fill in this section.")]
        public UserRole Role { get; set; }
      


    }
}
