using ToDoListService.Framework.Mappers;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;

namespace ToDoListService.Domain.Mappers;

public interface IToDoMapper: IMapper<ToDo, ToDoInputDto, ToDoOutputDto>
{
}
