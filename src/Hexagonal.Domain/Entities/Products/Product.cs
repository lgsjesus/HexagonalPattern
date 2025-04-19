using FluentValidation.Results;
using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Enums;

namespace Hexagonal.Domain.Entities.Products;

public sealed class Product : Entity<Guid>
{
    public new required Guid Id { get; init; }
    public string Name { get; private set; } = string.Empty;
    public Status Status { get; private set; }
    public decimal Price { get; private set; }

    public static Product CreateDisable(Guid id, string name, decimal price) => new()
    {
        Id = id,
        Name = name,
        Price = price,
        Status = Status.Disabled
    };
    public static Product Create(Guid id, string name, decimal price,Status status) => new()
    {
        Id = id,
        Name = name,
        Price = price,
        Status = status
    };
    public void Update(string name, Status status, decimal price)
    {
        Name = name;
        Status = status;
        Price = price;
    }

    public void ChangePrice(decimal price)
    {
        if(price < 0)
            throw new UserException("Price cannot be negative");
        Price = price;
    }
    public void Enable()
    {
        if(Price <= 0)
            throw new UserException("Price must be positive");
        
        Status = Status.Enabled;
    }
    public void Disable()
    {
        if(Price > 0)
            throw new UserException("Price must be zero or negative");
        
        Status = Status.Disabled;
    }
    public ValidationResult IsValid()
    {
        var productValidator = new ProductValidator();
        return productValidator.Validate(this);
    }
}