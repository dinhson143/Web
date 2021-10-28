using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Promotions
{
    public class PromotionCreateValidator : AbstractValidator<PromotionCreateModel>
    {
        public PromotionCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name events is required")
                .MaximumLength(200).WithMessage("Name events at max 200 charaters");
            RuleFor(x => x.FromDate).NotEmpty().WithMessage("From Date is required");
            RuleFor(x => x.ToDate).NotEmpty().WithMessage("From Date is required");
        }
    }
}