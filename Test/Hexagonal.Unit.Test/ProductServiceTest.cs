using FakeItEasy;
using FluentAssertions;
using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.Domain.Enums;
using Hexagonal.ProductService;
using Optional;

namespace Hexagonal.Unit.Test;

public class ProductServiceTest
{
    private readonly IProductService _service;
    private readonly IProductWriteRepository _repository;
    public ProductServiceTest()
    {
        _repository = A.Fake<IProductWriteRepository>();
        _service = new ProductServices(_repository);
    }

    [Fact]
    public void GetById_ProductExists_ReturnsProduct()
    {
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Test", 12);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some<Product>(product));
        
        _service.GetProductById(id).Should().BeEquivalentTo(product);
    }
    [Fact]
    public void GetById_ProductNotExists_ReturnsProduct()
    {
        var id = Guid.NewGuid();
        A.CallTo(()=> _repository.GetProduct(id))
            .Returns(  Option.None<Product>());
        Action check = () => _service.GetProductById(id);
        //Assert
        check.Should().Throw<UserException>("Product not found");
    }
    [Fact]
    public void Save_ProductPriceZero_ReturnsError()
    {
        var product = Product.Create(Guid.NewGuid(), "Test", 0);
        A.CallTo(() => _repository.Save(product)).Returns(product);
        Action check = () => _service.Create(product.Name,product.Price, Status.Enabled);
        //Assert
        check.Should().Throw<UserException>("Price must be positive");
    }
    [Fact]
    public void Save_ProductPriceGreatherZero_ReturnsSuccess()
    {
        var product = Product.Create(Guid.NewGuid(), "Test", 15);
        A.CallTo(() => _repository.Save(product)).Returns(product);
       _service.Create("Test", 15, Status.Enabled).Should().BeOfType<Product>();
    }
    [Fact]
    public void Enable_ProductPriceGreatherZero_ReturnsSuccess()
    {
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Test", 12);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some<Product>(product));

        Action check = () => _service.Enable(product);
        check.Should().NotThrow();
    }
    [Fact]
    public void Enable_ProductPriceZero_ReturnsError()
    {
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Test", 0);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some<Product>(product));

        Action check = () => _service.Enable(product);
        check.Should().Throw<UserException>("Price cannot be zero or negative");
    }
    [Fact]
    public void Disable_ProductPriceZero_ReturnsSuccess()
    {
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Test", 0);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some<Product>(product));

        Action check = () => _service.Disable(product);
        check.Should().NotThrow();
        
    }
    [Fact]
    public void Disable_ProductPriceGreatherZero_ReturnsError()
    {
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Test", 15);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some<Product>(product));

        Action check = () => _service.Disable(product);
        check.Should().Throw<UserException>("Price must to be zero or negative to Disable");
    }
}