using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Congtys
{
    public class CongtyCreateValidator : AbstractValidator<CongtyCreate>
    {
        public CongtyCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
               .MaximumLength(200).WithMessage("Name at max 200 charaters");

            RuleFor(x => x.Diachi).NotEmpty().WithMessage("Địa chỉ is required")
                    .MaximumLength(500).WithMessage("Địa chỉ at max 200 charaters");
            RuleFor(x => x.Masothue).NotEmpty().WithMessage("Mã số thuế is required")
                .MaximumLength(13).WithMessage("Mã số thuế must be less than 13 digits")
                .MinimumLength(10).WithMessage("Mã số thuế must be more than 10 digits");
            RuleFor(x => x.Sdt).NotEmpty().WithMessage("Số điện thoại is required").Length(10).WithMessage("Phone must be 10 digits");
        }
    }
}