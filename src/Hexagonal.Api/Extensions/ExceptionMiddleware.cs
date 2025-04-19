using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexagonal.Api.Controllers.Dtos;

namespace Hexagonal.Api.Extensions;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var erro = JsonSerializer.Serialize(ApiResponse.CreateError(ex.Message));
            var buffer = Encoding.UTF8.GetBytes(erro);
            await httpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}