using ToDoListService.Framework.Controllers;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Services;

namespace ToDoListService.Api.Controllers;

public class ToDosController(IToDoService service, IAuthenticatedUserFactory userFactory) : BaseSecuredCrudController<int, ToDoInputDto, ToDoOutputDto>(service, userFactory)
{
}
