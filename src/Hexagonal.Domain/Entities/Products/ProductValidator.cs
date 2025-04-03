using FluentValidation;
using Hexagonal.Domain.Enums;

namespace Hexagonal.Domain.Entities.Products;

public sealed class ProductValidator : AbstractValidator<Product>
{
   public ProductValidator()
   {
      RuleFor(product => product.Id).NotEmpty().WithMessage("Id Product cannot be empty");
      RuleFor(product => product.Name).NotEmpty().WithMessage("Name Product cannot be empty");
      When(product => product.Status == Status.Enabled, () =>
      {
         RuleFor(product => product.Price)
            .GreaterThan(0).WithMessage("Price must be positive");
      });
      When(product => product.Status == Status.Disabled, () =>
      {
         RuleFor(product => product.Price).LessThanOrEqualTo(0)
            .WithMessage("Price must be zero or negative");
      });
      RuleFor(product => product.Status).IsInEnum().WithMessage("Product must have a valid status");
   }
}