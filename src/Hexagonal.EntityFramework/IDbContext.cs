using Hexagonal.Domain.Entities.Comums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hexagonal.EntityFramework;

public interface IDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity, TId>() where TEntity : Entity<TId> where TId : struct;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}