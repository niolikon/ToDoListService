using ToDoListService.Framework.Entities;

namespace ToDoListService.Framework.Repositories;

public interface ICrudRepository<TEntity, TId> 
    where TEntity:BaseEntity<TId>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> ReadAllAsync();
    Task<IEnumerable<TEntity>> ReadAllAsync(Func<TEntity, bool> condition);
    Task<TEntity> ReadAsync(TId id);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TId id);
}
