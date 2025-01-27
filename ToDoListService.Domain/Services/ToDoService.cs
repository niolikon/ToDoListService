using Microsoft.AspNetCore.Identity;
using ToDoListService.Framework.Services;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Mappers;
using ToDoListService.Domain.Repositories;

namespace ToDoListService.Domain.Services;

public class ToDoService: BaseSecuredCrudService<ToDo, int, ToDoInputDto, ToDoOutputDto, User>, IToDoService
{
    public ToDoService(IToDoRepository repository, IToDoMapper mapper, UserManager<User> userManager) : base(repository, mapper, userManager) { }
}
