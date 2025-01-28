using ToDoListService.Framework.Services;
using ToDoListService.Domain.Dtos;
using ToDoListService.Framework.Security.Authentication;

namespace ToDoListService.Domain.Services;

public interface IToDoService: ISecuredCrudService<int, ToDoInputDto, ToDoOutputDto>
{
    Task<IEnumerable<ToDoOutputDto>> ReadAllPendingAsync(AuthenticatedUser user);
    Task<ToDoOutputDto> PatchAsync(int id, ToDoPatchDto patchDto, AuthenticatedUser user);
}
