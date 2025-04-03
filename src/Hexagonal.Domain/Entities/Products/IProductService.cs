using Hexagonal.Domain.Enums;

namespace Hexagonal.Domain.Entities.Products;

public interface IProductService
{
    Product GetProductById(Guid id);
    Product Create(string name,decimal price,Status status);
    void Enable(Product product);
    void Disable(Product product);
}