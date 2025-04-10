namespace Hexagonal.Api.Controllers.Dtos;

public sealed record Response
{
    public IReadOnlyList<string> Errors { get; set; }
    public bool Success => Errors.Count == 0;
    public object? Data { get; set; }
}