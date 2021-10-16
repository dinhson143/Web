using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Sliders
{
    public class SliderCreateValidator : AbstractValidator<SliderCreate>
    {
        public SliderCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name at max 200 charaters");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description at max 200 charaters");
            RuleFor(x => x.Url).NotEmpty().WithMessage("URL is required");
        }
    }
}