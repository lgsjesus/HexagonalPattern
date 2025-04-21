using System.Collections.ObjectModel;
using Optional;

namespace Hexagonal.Domain.Entities.Products;

public interface IProductRepository
{
    Task<Product> SaveProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task<Option<Product>> GetProduct(Guid productId);
    Task RemoveProduct(Guid productId);
    Task<Option<ReadOnlyCollection<Product>>> GetAllProduct();
}