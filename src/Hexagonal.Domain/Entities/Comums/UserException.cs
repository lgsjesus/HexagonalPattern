namespace Hexagonal.Domain.Entities.Comums;

public sealed class UserException(string? message) : Exception(message);