using Microsoft.AspNetCore.Identity;
using ToDoListService.Framework.Dtos;
using ToDoListService.Framework.Entities;
using ToDoListService.Framework.Exceptions.Persistence;
using ToDoListService.Framework.Exceptions.Rest;
using ToDoListService.Framework.Mappers;
using ToDoListService.Framework.Repositories;
using ToDoListService.Framework.Security.Authentication;

namespace ToDoListService.Framework.Services;

public class BaseSecuredCrudService<TEntity, TId, TInputDto, TOutputDto, TUser> : ISecuredCrudService<TId, TInputDto, TOutputDto>
    where TEntity : BaseOwnedEntity<TId, TUser>
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
    where TUser : IdentityUser
{
    protected ISecuredCrudRepository<TEntity, TId, TUser> _repository;
    protected IMapper<TEntity, TInputDto, TOutputDto> _mapper;
    protected UserManager<TUser> _userManager;

    public BaseSecuredCrudService(
        ISecuredCrudRepository<TEntity, TId, TUser> repository,
        IMapper<TEntity, TInputDto, TOutputDto> mapper,
        UserManager<TUser> userManager)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
    }

    protected async Task<TUser> GetOwner(AuthenticatedUser authenticatedUser)
    {
        TUser? user = await _userManager.FindByIdAsync(authenticatedUser.Id);
        if (user is null)
        {
            throw new UnauthorizedRestException("User unknown");
        }
        
        if (!object.Equals(user.UserName, authenticatedUser.UserName))
        {
            throw new UnauthorizedRestException("User unknown");
        }

        return user;
    }

    public async Task<TOutputDto> CreateAsync(TInputDto dto, AuthenticatedUser user)
    {
        TUser owner = await GetOwner(user);
        TEntity entity = _mapper.mapToEntity(dto);
        try
        {
            TEntity entityInDb = await _repository.CreateAsync(entity, owner);
            return _mapper.mapToOutputDto(entityInDb);
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not create entity");
        }
    }

    public async Task<IEnumerable<TOutputDto>> ReadAllAsync(AuthenticatedUser user)
    {
        TUser owner = await GetOwner(user);
        IEnumerable<TEntity> entities = await _repository.ReadAllAsync(owner);
        return entities.Select(_mapper.mapToOutputDto).ToList();
    }

    public async Task<TOutputDto> ReadAsync(TId id, AuthenticatedUser user)
    {
        TUser owner = await GetOwner(user);
        try
        {
            TEntity entity = await _repository.ReadAsync(id, owner);
            return _mapper.mapToOutputDto(entity);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (EntityOwnershipViolationException)
        {
            throw new UnauthorizedRestException("User not authorized to read entities under others' ownership");
        }
    }

    public async Task<TOutputDto> UpdateAsync(TId id, TInputDto dto, AuthenticatedUser user)
    {
        TUser owner = await GetOwner(user);
        TEntity entityWithUpdatedData = _mapper.mapToEntity(dto);
        entityWithUpdatedData.Id = id;

        try
        {
            TEntity entityUpdated = await _repository.UpdateAsync(entityWithUpdatedData, owner);
            return _mapper.mapToOutputDto(entityUpdated);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (EntityOwnershipViolationException)
        {
            throw new UnauthorizedRestException("User not authorized to update entities under others' ownership");
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not update entity");
        }
    }

    public async Task DeleteAsync(TId id, AuthenticatedUser user)
    {
        TUser owner = await GetOwner(user);

        try
        {
            await _repository.DeleteAsync(id, owner);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (EntityOwnershipViolationException)
        {
            throw new UnauthorizedRestException("User not authorized to delete entities under others' ownership");
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not delete entity");
        }
    }
}
