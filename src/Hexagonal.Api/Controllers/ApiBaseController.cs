using System.Diagnostics.CodeAnalysis;
using Hexagonal.Api.Controllers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal.Api.Controllers;

public abstract class ApiBaseController : ControllerBase
{
    public IActionResult HandleResponse([NotNull] Response response)
    {
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
}