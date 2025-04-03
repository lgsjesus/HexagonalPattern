using FluentValidation;

namespace Hexagonal.Domain.Entities.Products;

public sealed class ProductValidator : AbstractValidator<Product>
{
   public ProductValidator()
   {
      RuleFor(product => product.Id).NotEmpty().WithMessage("Id Product cannot be empty");
      RuleFor(product => product.Name).NotEmpty().WithMessage("Name Product cannot be empty");
      RuleFor(product => product.Price).GreaterThan(0).WithMessage("Price must be positive");
      RuleFor(product => product.Status).IsInEnum().WithMessage("Product must have a valid status");
   }
}