using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Categories
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreate>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name at max 200 charaters");

            RuleFor(x => x.SeoDescription).NotEmpty().WithMessage("SeoDescription is required")
                .MaximumLength(500).WithMessage("SeoDescription at max 200 charaters");
            RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("SeoTitle is required");
            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("SeoAlias is required");
        }
    }
}