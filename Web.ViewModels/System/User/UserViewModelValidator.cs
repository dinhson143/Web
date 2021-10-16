using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.System.User
{
    public class UserViewModelValidator : AbstractValidator<UserViewModelWeb>
    {
        public UserViewModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required")
               .MaximumLength(200).WithMessage("FirstName at max 200 charaters");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required")
                .MaximumLength(200).WithMessage("LastName at max 200 charaters");

            RuleFor(x => x.Dob).NotEmpty().WithMessage("Birthday is required")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Age can not greater than 100 years");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Value is not Email");

            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone Number is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        }
    }
}