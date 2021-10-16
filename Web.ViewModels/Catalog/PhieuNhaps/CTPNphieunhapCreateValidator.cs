using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PhieuNhaps
{
    public class CTPNphieunhapCreateValidator : AbstractValidator<CTPhieuNhapCreateViewModel>
    {
        public CTPNphieunhapCreateValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Vui lòng chọn sản phẩm is required");
            RuleFor(x => x.SizeId).NotEmpty().WithMessage("Vui lòng chọn sản phẩm is required");
            RuleFor(x => x.Giaban).NotEmpty().WithMessage("Giá bán is required");
            RuleFor(x => x.Soluong).NotEmpty().WithMessage("Số lượng is required");
            RuleFor(x => x.Dongia).NotEmpty().WithMessage("Đơn giá is required");
        }
    }
}