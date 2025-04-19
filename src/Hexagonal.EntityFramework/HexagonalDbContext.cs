using Hexagonal.Domain.Entities.Products;
using Hexagonal.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.EntityFramework;

public class HexagonalDbContext(DbContextOptions options): DbContext(options)
{
    public DbSet<Product> Product { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new ProductMap().Configure(modelBuilder.Entity<Product>());
    }
}