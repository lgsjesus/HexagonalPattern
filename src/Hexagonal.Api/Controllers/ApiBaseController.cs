using System.Diagnostics.CodeAnalysis;
using Hexagonal.Api.Controllers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal.Api.Controllers;

[Route("api/[controller]")]
public abstract class ApiBaseController : ControllerBase
{
    protected  IActionResult HandleResponse(object? retorno = null) 
    {
        try
        {
            return Ok(ApiResponse.CreateSuccess(retorno));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.CreateError(ex.Message));
        }
    }
}