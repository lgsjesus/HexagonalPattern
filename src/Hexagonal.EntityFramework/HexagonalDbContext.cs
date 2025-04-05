using Hexagonal.Domain.Entities.Comums;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.EntityFramework;

public class HexagonalDbContext(DbContextOptions options) : DbContext(options) , IDbContext
{
    public DbSet<TEntity> Set<TEntity, TId>() where TEntity : Entity<TId> where TId : struct => base.Set<TEntity>();
}