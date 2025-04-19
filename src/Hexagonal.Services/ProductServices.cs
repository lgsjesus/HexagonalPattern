using System.Collections.ObjectModel;
using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.Domain.Enums;

namespace Hexagonal.ProductService;

public sealed class ProductServices(IProductRepository productRepository) : IProductService
{
    public async Task<Product> GetProductById(Guid id)
    {
        var item = await productRepository.GetProduct(id);
      return item.Match(some: product => product,
          none: () => throw new UserException("Product not found"));
    }

    public async Task<ReadOnlyCollection<Product>> GetAllProducts()
        => (await productRepository.GetAllProduct())
            .ValueOr( new ReadOnlyCollection<Product>(Enumerable.Empty<Product>().ToList()) );

    public async Task<Product> Create(string name, decimal price, Status status)
    {
        var produto = Product.CreateDisable(Guid.NewGuid(), name, price);
        if (status == Status.Enabled)
            produto.Enable();
        else
            produto.Disable();
        
        var result = produto.IsValid();
        if (result.IsValid)
        {
            await productRepository.SaveProduct(produto);
            return produto;
        }
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }

    public async Task<Product> Update(Guid id, string name, decimal price, Status status)
    {
        var produto = Product.Create(id, name, price, status);
        var result = produto.IsValid();
        if (result.IsValid)
        {
            await productRepository.UpdateProduct(produto);
            return produto;
        }
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }
    public async Task Enable(Product product)
    {
        var productExistente = await GetProductById(product.Id);
        productExistente.Enable();
        var result = productExistente.IsValid();
        if (result.IsValid)
        {
            await productRepository.SaveProduct(productExistente);
            return;
        }
        
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }
    public async Task Disable(Product product)
    {
        var productExistente = await GetProductById(product.Id);
        productExistente.Disable();
        var result = productExistente.IsValid();
        if (result.IsValid)
        {
            await productRepository.SaveProduct(productExistente);
            return;
        }
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }
}