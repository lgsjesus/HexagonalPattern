using Optional;

namespace Hexagonal.Domain.Entities.Products;

public interface IProductWriteRepository
{
    Product Save(Product product);
    Option<Product> GetById(Guid id);
}