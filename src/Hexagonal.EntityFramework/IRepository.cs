using Hexagonal.Domain.Entities.Comums;
using Optional;

namespace Hexagonal.EntityFramework;

public interface IRepository<T,TId> where T : Entity<TId>  where TId : struct
{
    Task<Option<T>> GetById(TId id); 
    Task DeleteById(TId id); 
    Task<Option<T>> Update(T entity); 
    Task<Option<T>> Create(T entity); 
}