using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoListService.Framework.Entities;

namespace ToDoListService.Framework.Repositories;

public interface ISecuredCrudRepository<TEntity, TId, TUser> 
    where TEntity : BaseOwnedEntity<TId, TUser>
    where TUser : IdentityUser
{
    Task<TEntity> CreateAsync(TEntity entity, TUser owner);
    Task<IEnumerable<TEntity>> ReadAllAsync(TUser owner);
    Task<IEnumerable<TEntity>> ReadAllAsync(TUser owner, Func<TEntity, bool> condition);
    Task<TEntity> ReadAsync(TId id, TUser owner);
    Task<TEntity> UpdateAsync(TEntity entity, TUser owner);
    Task DeleteAsync(TId id, TUser owner);
}
