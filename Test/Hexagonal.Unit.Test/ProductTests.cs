using FluentAssertions;
using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.Domain.Enums;

namespace Hexagonal.Unit.Test;

public class ProductTests
{
    [Fact]
    public void TestProductEnableWithSucced()
    {
        var product = Product.CreateDisable(Guid.NewGuid(), "Product 1", (decimal) 15.56);
        Action check = () => product.Enable();
        //Assert
        check.Should().NotThrow();
        Assert.True( product.Status == Status.Enabled);
    }
    [Fact]
    public void TestProductEnableWithErrorPriceZero()
    {
        var product = Product.CreateDisable(Guid.NewGuid(), "Product 1", -5);
        Action check = () => product.Enable();
        //Assert
        check.Should().Throw<UserException>("Price cannot be zero or negative");
    }
    [Fact]
    public void TestProductIsValidWithSucceced()
    {
        var product = Product.CreateDisable(Guid.NewGuid(), "Product 1", 15);
        product.Enable();
        var valid = product.IsValid();
        //Assert
        Assert.Empty(valid.Errors);
    }
    [Fact]
    public void TestProductNotIsValidWithoutName()
    {
        var product = Product.CreateDisable(Guid.NewGuid(), string.Empty, 15);
        product.Enable();
        var valid = product.IsValid();
        //Assert
        Assert.Equal("Name Product cannot be empty", valid.Errors[0].ErrorMessage);
    }
    [Fact]
    public void TestProductNotIsValidWithoutId()
    {
        var product = Product.CreateDisable(Guid.Empty, "product 1", 15);
        product.Enable();
        var valid = product.IsValid();
        //Assert
        Assert.Equal("Id Product cannot be empty", valid.Errors[0].ErrorMessage);
    }
}
