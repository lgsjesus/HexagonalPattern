using System.Diagnostics.CodeAnalysis;
using Hexagonal.Api.Controllers.Dtos;
using Hexagonal.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService service) : ApiBaseController
{
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllProduct()
        => HandleResponse( (await service.GetAllProducts())
            .Select(c => (ProductDto) c).AsEnumerable());
    [HttpGet("Get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid id)
        => HandleResponse(await service.GetProductById(id));
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateProduct(ProductDto dto)
        => HandleResponse(await service.Create(dto.Name, dto.Price, dto.Status));
    
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([NotNull] ProductDto dto)
        => HandleResponse( await service.Update( Guid.Parse(dto.Id!),dto.Name, dto.Price, dto.Status));
    
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    => HandleResponse(await service.RemoveProductById(id));
}