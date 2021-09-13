using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.System.User
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required")
                .MinimumLength(200).WithMessage("FirstName at least 200 charaters");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required")
                .MinimumLength(200).WithMessage("LastName at least 200 charaters");

            RuleFor(x => x.Dob).NotEmpty().WithMessage("Birthday is required")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Age can not greater than 100 years");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Value is not Email");

            RuleFor(x => x.Phonenumber).NotEmpty().WithMessage("Phonenumber is required");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                                    .MinimumLength(6).WithMessage("Password is at list 6 characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword is required");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("ConfirmPassword is incorrect");
                }
            });
        }
    }
}