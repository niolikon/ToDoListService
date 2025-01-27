using ToDoListService.Framework.Repositories;
using ToDoListService.Domain.Entities;

namespace ToDoListService.Domain.Repositories;

public interface IToDoRepository : ISecuredCrudRepository<ToDo, int, User>
{
}
