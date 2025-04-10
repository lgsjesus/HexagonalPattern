using Hexagonal.Domain.Entities.Products;
using Hexagonal.Domain.Enums;

namespace Hexagonal.Api.Controllers.Dtos;

public sealed record ProductDto
{
    public static ProductDto Create(Guid id, string name, decimal price, Status status) => new()
    {
        Id = id,
        Name = name,
        Price = price,
        Status = status,
    };
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; init; }
    public required decimal Price { get; init; } = decimal.Zero;
    public required Status Status{ get; init; }
    
    public static implicit operator Product(ProductDto dto) => 
        Product.Create(dto.Id, dto.Name, dto.Price,dto.Status);
    
    public static implicit operator ProductDto(Product entity) => 
        ProductDto.Create(entity.Id, entity.Name, entity.Price,entity.Status);
}