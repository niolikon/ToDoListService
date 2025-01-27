using Microsoft.AspNetCore.Identity;
using ToDoListService.Framework.Services;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Mappers;
using ToDoListService.Domain.Repositories;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Framework.Exceptions.Persistence;
using ToDoListService.Framework.Exceptions.Rest;

namespace ToDoListService.Domain.Services;

public class ToDoService : BaseSecuredCrudService<ToDo, int, ToDoInputDto, ToDoOutputDto, User>, IToDoService
{
    public ToDoService(IToDoRepository repository, IToDoMapper mapper, UserManager<User> userManager) : base(repository, mapper, userManager) { }

    public async Task<IEnumerable<ToDoOutputDto>> ReadAllPendingAsync(AuthenticatedUser user)
    {
        User owner = await GetOwner(user);
        IEnumerable<ToDo> entities = await _repository.ReadAllAsync(owner, e => e.IsCompleted ?? false);
        return entities.Select(_mapper.mapToOutputDto).ToList();
    }

    public async Task<ToDoOutputDto> PatchAsync(int id, ToDoPatchDto patchDto, AuthenticatedUser user)
    {
        User owner = await GetOwner(user);
        ToDo entityWithUpdatedData = new()
        {
            Id = id,
            IsCompleted = patchDto.IsCompleted,
            DueDate = patchDto.DueDate
        };

        try
        {
            ToDo entityInDb = await _repository.ReadAsync(id, owner);
            if (entityInDb.IsCompleted ?? false)
            {
                return _mapper.mapToOutputDto(entityInDb);
            }

            ToDo entityUpdated = await _repository.UpdateAsync(entityWithUpdatedData, owner);
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
}
