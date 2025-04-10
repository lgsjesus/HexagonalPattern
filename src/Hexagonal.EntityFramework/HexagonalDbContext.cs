using Hexagonal.Domain.Entities.Comums;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.EntityFramework;

public class HexagonalDbContext(DbContextOptions options): DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(new MySqlServerVersion("5.7"));
    }

    public DbSet<Product> Product { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new ProductMap().Configure(modelBuilder.Entity<Product>());
    }
}