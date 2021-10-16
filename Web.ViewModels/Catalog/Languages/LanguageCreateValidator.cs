using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Languages
{
    public class LanguageCreateValidator : AbstractValidator<LanguageCreate>
    {
        public LanguageCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
               .MaximumLength(200).WithMessage("Name at max 200 charaters");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Mã ngôn ngữ is required").MaximumLength(5).WithMessage("Mã ngôn ngữ must be least 5 digits");
        }
    }
}