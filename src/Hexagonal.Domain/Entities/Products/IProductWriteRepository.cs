using Optional;

namespace Hexagonal.Domain.Entities.Products;

public interface IProductWriteRepository
{
    Task<Product> Save(Product product);
    Task<Option<Product>> GetProduct(Guid productId);
}