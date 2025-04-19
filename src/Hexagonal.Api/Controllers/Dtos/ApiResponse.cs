namespace Hexagonal.Api.Controllers.Dtos;

public sealed record ApiResponse
{
    public string? Error { get; set; }
    public bool Success {get; set;}
    public object? Data { get; set; }
    
    public static ApiResponse CreateSuccess(in object? data) => new()
    {
        Data = data,
        Success = true,
        Error = null,
    };
    public static ApiResponse CreateError( in string? error) => new()
    {
        Data = null,
        Success = false,
        Error = error,
    };
}