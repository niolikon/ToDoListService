using ToDoListService.Framework.Controllers;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToDoListService.Framework.Exceptions.Rest;

namespace ToDoListService.Api.Controllers;

public class ToDosController(IToDoService service, IAuthenticatedUserFactory userFactory) : BaseSecuredCrudController<int, ToDoInputDto, ToDoOutputDto>(service, userFactory)
{
    [HttpGet("pending")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ToDoOutputDto>>> ReadAllPendingAsync()
    {
        AuthenticatedUser user = userFactory.BuildAuthenticatedUser(HttpContext);
        IEnumerable<ToDoOutputDto> result = await service.ReadAllPendingAsync(user);
        return Ok(result);
    }

    [HttpPatch("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ToDoOutputDto>> PatchAsync(int id, [FromBody] ToDoPatchDto patchDto)
    {
        AuthenticatedUser user = userFactory.BuildAuthenticatedUser(HttpContext);

        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        ToDoOutputDto result = await service.PatchAsync(id, patchDto, user);
        return Ok(result);
    }
}
