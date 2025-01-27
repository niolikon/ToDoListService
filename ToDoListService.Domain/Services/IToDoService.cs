using ToDoListService.Framework.Services;
using ToDoListService.Domain.Dtos;

namespace ToDoListService.Domain.Services;

public interface IToDoService: ISecuredCrudService<int, ToDoInputDto, ToDoOutputDto>
{
}
