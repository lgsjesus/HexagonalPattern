using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.Domain.Enums;

namespace Hexagonal.ProductService;

public sealed class ProductServices(IProductWriteRepository productWriteRepository) : IProductService
{
    public Product GetProductById(Guid id)
    {
      return productWriteRepository.GetById(id).Match(some: product => product,
          none: () => throw new UserException("Product not found"));
    }
    public Product Create(string name, decimal price, Status status)
    {
        var produto = Product.Create(Guid.NewGuid(), name, price);
        if (status == Status.Enabled)
            produto.Enable();
        else
            produto.Disable();
        
        var result = produto.IsValid();
        if (result.IsValid)
        {
            productWriteRepository.Save(produto);
            return produto;
        }
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }
    public void Enable(Product product)
    {
        var productExistente = GetProductById(product.Id);
        productExistente.Enable();
        var result = productExistente.IsValid();
        if (result.IsValid)
        {
            productWriteRepository.Save(productExistente);
            return;
        }
        
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }
    public void Disable(Product product)
    {
        var productExistente = GetProductById(product.Id);
        productExistente.Disable();
        var result = productExistente.IsValid();
        if (result.IsValid)
        {
            productWriteRepository.Save(productExistente);
            return;
        }
        throw new UserException( result.Errors.
            Select(c=> c.ErrorMessage).CommAggregate());
    }
}