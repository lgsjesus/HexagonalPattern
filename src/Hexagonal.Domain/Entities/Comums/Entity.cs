namespace Hexagonal.Domain.Entities.Comums;

public class Entity<T> where T : struct
{
    public T Id { get; set; }
}