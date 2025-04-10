using Hexagonal.Api.Controllers.Dtos;
using Hexagonal.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProductController(IProductService service) : ApiBaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<ProductDto>> GetAllProduct()
        => (await service.GetAllProducts())
            .Select(c => (ProductDto) c).AsEnumerable();
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ProductDto> GetProductById(Guid id)
        => await service.GetProductById(id);
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ProductDto> CreateProduct(ProductDto dto)
        => await service.Create(dto.Name, dto.Price, dto.Status);
}