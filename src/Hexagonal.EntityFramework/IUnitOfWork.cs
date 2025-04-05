using Hexagonal.Domain.Entities.Comums;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.EntityFramework;

public interface IUnitOfWork : IDisposable
{
    Task<bool> Commit();
}