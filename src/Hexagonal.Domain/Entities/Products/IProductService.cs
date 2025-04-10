using System.Collections.ObjectModel;
using Hexagonal.Domain.Enums;

namespace Hexagonal.Domain.Entities.Products;

public interface IProductService
{
    Task<Product> GetProductById(Guid id);
    Task<ReadOnlyCollection<Product>> GetAllProducts();
    Task<Product> Create(string name,decimal price,Status status);
    Task Enable(Product product);
    Task Disable(Product product);
}