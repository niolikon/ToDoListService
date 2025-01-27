using ToDoListService.Framework.Repositories;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Repositories;

namespace ToDoListService.Infrastructure.Repositories;

public class ToDoRepository : BaseSecuredCrudRepository<ToDo, int, User>, IToDoRepository
{
    public ToDoRepository(AppDbContext context) : base(context) {}
}
