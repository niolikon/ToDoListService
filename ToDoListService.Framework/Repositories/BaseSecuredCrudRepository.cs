using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoListService.Framework.Entities;
using ToDoListService.Framework.Exceptions.Persistence;

namespace ToDoListService.Framework.Repositories;

public class BaseSecuredCrudRepository<TEntity, TId, TUser> : ISecuredCrudRepository<TEntity, TId, TUser>
    where TEntity : BaseOwnedEntity<TId, TUser>
    where TUser : IdentityUser
{
    protected IdentityDbContext<TUser> _dbContext;

    public BaseSecuredCrudRepository(IdentityDbContext<TUser> dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> CreateAsync(TEntity entity, TUser owner)
    {
        entity.Owner = owner;

        EntityEntry<TEntity> result = await _dbContext.Set<TEntity>().AddAsync(entity);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("CreateAsync created no rows");
        }

        return result.Entity;
    }

    public async Task<IEnumerable<TEntity>> ReadAllAsync(TUser owner)
    {
        return await _dbContext.Set<TEntity>().Where(e => e.Owner.Equals(owner)).ToListAsync();
    }

    public async Task<TEntity> ReadAsync(TId id, TUser owner)
    {
        TEntity? entityInDb = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entityInDb == null)
        {
            throw new EntityNotFoundException($"Could not find {typeof(TEntity)} with id {id}");
        }
        if (entityInDb.Owner != owner)
        {
            throw new EntityOwnershipViolationException($"{typeof(TEntity)} does not belong to {owner}");
        }

        return entityInDb;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, TUser owner)
    {
        TEntity entityInDatabase = await ReadAsync(entity.Id, owner);
        entityInDatabase.CopyFrom(entity);

        _dbContext.Set<TEntity>().Update(entityInDatabase);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("UpdateAsync updated no rows");
        }

        return entityInDatabase;
    }

    public async Task DeleteAsync(TId id, TUser owner)
    {
        TEntity entityInDatabase = await ReadAsync(id, owner);

        _dbContext.Set<TEntity>().Remove(entityInDatabase);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("DeleteAsync deleted no rows");
        }
    }
}
