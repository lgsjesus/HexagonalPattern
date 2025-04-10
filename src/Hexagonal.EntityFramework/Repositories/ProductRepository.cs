using System.Collections.ObjectModel;
using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Entities.Products;
using Optional;

namespace Hexagonal.EntityFramework.Repositories;

public sealed class ProductRepository(HexagonalDbContext context, IUnitOfWork unitOfWork) 
    : Repository<Product, Guid>(context,unitOfWork), IProductRepository
{
    public async  Task<Product> SaveProduct(Product product)
    => (await Create(product))
               .Match(some: prod => prod, none: () => throw new UserException("Product not created")) ;
    public async Task<Product> UpdateProduct(Product product)
    => (await Update(product))
        .Match(some: prod => prod, none: () => throw new UserException("Product not updated with new values")); 

    public async Task<Option<Product>> GetProduct(Guid productId)
    {
        return await GetById(productId);
    }

    public async Task<Option<ReadOnlyCollection<Product>>> GetAllProduct()
    => await GetAll();
}