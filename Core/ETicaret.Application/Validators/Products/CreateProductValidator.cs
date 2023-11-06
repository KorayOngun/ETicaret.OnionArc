using ETicaret.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("lütfen ürün adını boş geçmeyiniz")
                .MaximumLength(150)
                .MinimumLength(5)
                .WithMessage("lütfen üürn adını 5 ile 150 karakter arasında giriniz ");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("lütfen stock bilgisini boş geçmeyiniz")
                .Must(s => s >= 0)
                .WithMessage("lütfen stock bilgisini negatif değer vermeyin");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                .WithMessage("lütfen fiyat bilgisini boş geçmeyiniz")
                .Must(s => s >= 0)
                .WithMessage("lütfen fiyat bilgisini negatif değer vermeyin");
        }
    }
}
