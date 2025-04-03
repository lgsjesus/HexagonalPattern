namespace Hexagonal.Domain.Entities.Comums;

public static class StringFunctions
{
    public static string CommAggregate(this IEnumerable<string> item) => item.Aggregate((a, b) =>
        !string.IsNullOrEmpty(b) ?  $"{a} , {b}" : a);
}