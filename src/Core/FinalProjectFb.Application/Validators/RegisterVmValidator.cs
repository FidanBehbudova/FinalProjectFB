using FinalProjectFb.Application.ViewModels.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Validators
{
    public class RegisterVmValidator : AbstractValidator<RegisterVM>
    {
        public RegisterVmValidator()
        {
            RuleFor(x => x.Email).Must(CheckEmail);

            RuleFor(x => x.Password).Must(CheckPassword);

            RuleFor(x => x.Username).NotEmpty().WithMessage("You have to fill in this section.")
              .MaximumLength(100).WithMessage("herflere ekonomiya")
              .MinimumLength(8).WithMessage("The length of the name cannot be shorter than 8 letters.");

            RuleFor(x => x.Name).NotEmpty().WithMessage("You have to fill in this section.")
              .MaximumLength(50).WithMessage("herflere ekonomiya")
              .MinimumLength(2).WithMessage("The length of the name cannot be shorter than 2 letters.");

            RuleFor(x => x.Surname).NotEmpty().WithMessage("You have to fill in this section.")
              .MaximumLength(50).WithMessage("herflere ekonomiya")
              .MinimumLength(2).WithMessage("The length of the name cannot be shorter than 2 letters.");

            RuleFor(x => x)
                .Must(x => x.ConfirmPassword == x.Password);
        }
        public bool CheckEmail(string email)
        {
            if (email.Length < 8 || email.Length > 100)
            {
                return false;
            }

            string emailregex = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            return Regex.IsMatch(email, emailregex);
        }
        public bool CheckPassword(string password)
        {
            if (password.Length < 8 || password.Length > 100)
            {
                return false;
            }

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;

            foreach (char item in password)
            {
                if (char.IsUpper(item))
                {
                    hasUpper = true;
                }
                else if (char.IsLower(item))
                {
                    hasLower = true;
                }
                else if (char.IsDigit(item))
                {
                    hasDigit = true;
                }
            }

            return hasUpper && hasLower && hasDigit;
        }

    }
}
