using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class UpdatePriceViewModelValidator : AbstractValidator<UpdatePriceViewModel>
    {
        public UpdatePriceViewModelValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Vui lòng chọn sản phẩm is required");
            RuleFor(x => x.SizeId).NotEmpty().WithMessage("Vui lòng chọn sản phẩm is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá bán is required");
        }
    }
}