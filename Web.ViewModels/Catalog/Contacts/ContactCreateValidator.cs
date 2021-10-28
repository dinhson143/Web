using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Contacts
{
    public class ContactCreateValidator : AbstractValidator<ContactCreate>
    {
        public ContactCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name at max 200 charaters");

            RuleFor(x => x.Message).MaximumLength(500).WithMessage("Message at max 200 charaters");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
              .EmailAddress().WithMessage("Value is not Email");
        }
    }
}