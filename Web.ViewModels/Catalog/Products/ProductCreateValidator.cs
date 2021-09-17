using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Products
{
    public class ProductCreateValidator : AbstractValidator<ProductCreate>
    {
        public ProductCreateValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name at max 200 charaters");

            RuleFor(x => x.Details).NotEmpty().WithMessage("Details is required")
                .MaximumLength(500).WithMessage("Details at max 200 charaters");

            RuleFor(x => x.SeoDescription).NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("SeoTitle is required");
            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("SeoAlias is required");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("OriginalPrice is required");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Image is required");
        }
    }
}