using FluentAssertions;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.EntityFramework;
using Hexagonal.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hexagonal.Unit.Test;

public sealed class ProductRepositoryTest : IDisposable
{
    private readonly HexagonalRepositoryTest _context;
    private readonly IProductWriteRepository _productWriteRepository;
    public ProductRepositoryTest()
    {
        var contextOptions = new DbContextOptionsBuilder<HexagonalDbContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
       
       _context = new HexagonalRepositoryTest(contextOptions);
       _context.Database.EnsureDeleted();
       _context.Database.EnsureCreated();
       IUnitOfWork unitOfWork = new UnitOfWork(_context);
       _productWriteRepository = new ProductRepository(_context, unitOfWork);
       _context.SaveChanges();
 
    }

    [Fact]
    public async Task TestAddProductAsync()
    {
        var product = Product.Create(Guid.Empty, "Product 1",15);
        product.Enable();
        var result = await _productWriteRepository.Save(product);
        result.Id.Should().NotBeEmpty();
    }
    [Fact]
    public async Task TestIfGetProductAfterSaveAsync()
    {
        var product = Product.Create(Guid.Empty, "Product 1",15);
        product.Enable();
        var result = await _productWriteRepository.Save(product);
        var queryProduct = (await _productWriteRepository.GetProduct(result.Id)).ValueOr( Product.Create(Guid.Empty, "Product 2",15) );
       
        Assert.Equal(result.Id,queryProduct.Id);
        Assert.Equal(result.Name,queryProduct.Name);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
public sealed class HexagonalRepositoryTest(DbContextOptions options) : HexagonalDbContext(options)
{
    
}