using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().NotNull().WithMessage("Lütfen ürün adını boş geçmeyiniz.").MaximumLength(150).MinimumLength(2).WithMessage("Lütfen ürün adını 5-150 karakter arasında giriniz.");
            RuleFor(x => x.Stock).NotEmpty().NotNull().WithMessage("Lütfen stok bilgisini boş geçmeyiniz").Must(x => x >= 0).WithMessage("Stok bilgisi negatif olamaz");
            RuleFor(x => x.Price).NotEmpty().NotNull().WithMessage("Lütfen fiyat bilgisini boş geçmeyiniz").Must(x => x >= 0).WithMessage("Fiyat bilgisi negatif olamaz");
        }
    }
}
