using System.Collections.ObjectModel;
using Optional;

namespace Hexagonal.Domain.Entities.Products;

public interface IProductRepository
{
    Task<Product> Save(Product product);
    Task<Option<Product>> GetProduct(Guid productId);
    Task<Option<ReadOnlyCollection<Product>>> GetAllProduct();
}