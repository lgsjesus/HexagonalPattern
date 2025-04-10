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
    private readonly IProductRepository _repository;
    public ProductServiceTest()
    {
        _repository = A.Fake<IProductRepository>();
        _service = new ProductServices(_repository);
    }

    [Fact]
    public async Task GetById_ProductExists_ReturnsProduct()
    {
        var id = Guid.NewGuid();
        var product = Product.CreateDisable(id, "Test", 12);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some(product));

        var resultProduct = await _service.GetProductById(id);
        resultProduct.Should().BeEquivalentTo(product);
    }
    [Fact]
    public async Task GetById_ProductNotExists_ReturnsProduct()
    {
        var id = Guid.NewGuid();
        A.CallTo(()=> _repository.GetProduct(id))
            .Returns(  Option.None<Product>());
        Func<Task> check = async () => await _service.GetProductById(id);
        //Assert
       await check.Should().ThrowAsync<UserException>().WithMessage("Product not found");
    }
    [Fact]
    public async Task Save_ProductPriceZero_ReturnsError()
    {
        var product = Product.CreateDisable(Guid.NewGuid(), "Test", 0);
        A.CallTo(() => _repository.SaveProduct(product)).Returns(product);
        Func<Task> check = async () => await  _service.Create(product.Name,product.Price, Status.Enabled);
        //Assert
        await check.Should().ThrowAsync<UserException>().WithMessage("Price must be positive");
    }
    [Fact]
    public async Task Save_ProductPriceGreatherZero_ReturnsSuccess()
    {
        var product = Product.CreateDisable(Guid.NewGuid(), "Test", 15);
        A.CallTo(() => _repository.SaveProduct(product)).Returns(product);
        Func<Task> check = async () => await  _service.Create("Test", 15, Status.Enabled);
        await check.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Enable_ProductPriceGreatherZero_ReturnsSuccess()
    {
        var id = Guid.NewGuid();
        var product = Product.CreateDisable(id, "Test", 12);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some(product));

        Func<Task> check = async () => await  _service.Enable(product);
        await check.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Enable_ProductPriceZero_ReturnsError()
    {
        var id = Guid.NewGuid();
        var product = Product.CreateDisable(id, "Test", 0);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some(product));

        Func<Task> check = async () => await _service.Enable(product);
        await check.Should().ThrowAsync<UserException>().WithMessage("Price must be positive");
    }
    [Fact]
    public async Task Disable_ProductPriceZero_ReturnsSuccess()
    {
        var id = Guid.NewGuid();
        var product = Product.CreateDisable(id, "Test", 0);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some(product));

        Func<Task> check = async () => await _service.Disable(product);
        await check.Should().NotThrowAsync();
        
    }
    [Fact]
    public async Task Disable_ProductPriceGreatherZero_ReturnsError()
    {
        var id = Guid.NewGuid();
        var product = Product.CreateDisable(id, "Test", 15);
        A.CallTo(()=> _repository.GetProduct(A<Guid>._))
            .Returns(  Option.Some(product));

        Func<Task> check = async () =>await _service.Disable(product);
       await check.Should().ThrowAsync<UserException>().WithMessage("Price must be zero or negative");
    }
    
    [Fact]
    public async Task Update_ProductPriceZeroAndEnable_ReturnsError()
    {
        var product = Product.Create(Guid.NewGuid(), "Test", 0,Status.Enabled);
        A.CallTo(() => _repository.UpdateProduct(product)).Returns(product);
        Func<Task> check = async () => await  _service.Update(Guid.NewGuid(), "Test", 0,Status.Enabled);
        //Assert
        await check.Should().ThrowAsync<UserException>().WithMessage("Price must be positive");
    }
    [Fact]
    public async Task Update_ProductPriceZeroAndDisable_ReturnsError()
    {
        Func<Task> check = async () => await  _service.Update(Guid.NewGuid(), "Test", 10,Status.Disabled);
        //Assert
        await check.Should().ThrowAsync<UserException>().WithMessage("Price must be zero or negative");
    }
}