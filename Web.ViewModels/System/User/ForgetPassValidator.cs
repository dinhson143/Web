using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.System.User
{
    public class ForgetPassValidator : AbstractValidator<ForgetPassViewModel>
    {
        public ForgetPassValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                                   .MinimumLength(6).WithMessage("Password is at list 6 characters");
        }
    }
}