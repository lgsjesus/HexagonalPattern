using System.Collections.ObjectModel;
using Hexagonal.Domain.Entities.Comums;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Hexagonal.EntityFramework;

public class Repository<TEntity, TId>(HexagonalDbContext context, IUnitOfWork unitOfWork) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct
{
    private DbSet<TEntity> DbSet => context.Set<TEntity>();
    public async Task<Option<TEntity>> GetById(TId id)
    {
        var item = await DbSet.FindAsync(id);
        return item == null ? Option.None<TEntity>() : Option.Some(item);
    }
    public async Task DeleteById(TId id)
    {
        TEntity entityToDelete = await DbSet.FindAsync(id) ?? throw new UserException($"Not found item by Id {id}");
        await Delete(entityToDelete);
    }

    private async Task Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            DbSet.Attach(entityToDelete);
        }
        DbSet.Remove(entityToDelete);
        await unitOfWork.Commit();
    }
    public async Task<Option<TEntity>> Update(TEntity entity)
    {
        DbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        return await unitOfWork.Commit() ? Option.Some(entity) : Option.None<TEntity>();
    }

    public async Task<Option<TEntity>> Create(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        return await unitOfWork.Commit() ? Option.Some(entity) : Option.None<TEntity>();
    }

    public async Task<Option<ReadOnlyCollection<TEntity>>> GetAll()
    {
        var items = await DbSet.ToListAsync();
        return items.Any() ? Option.Some(items.AsReadOnly()) : Option.None<ReadOnlyCollection<TEntity>>();
    }
}