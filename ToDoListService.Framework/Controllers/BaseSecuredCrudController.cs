using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoListService.Framework.Dtos;
using ToDoListService.Framework.Exceptions.Rest;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Framework.Services;

namespace ToDoListService.Framework.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseSecuredCrudController<TId, TInputDto, TOutputDto> : ControllerBase
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
{
    protected ISecuredCrudService<TId, TInputDto, TOutputDto> _service;
    protected IAuthenticatedUserFactory _userFactory;

    public BaseSecuredCrudController(
        ISecuredCrudService<TId, TInputDto, TOutputDto> service, 
        IAuthenticatedUserFactory userFactory)
    {
        _service = service;
        _userFactory = userFactory;
    }

    [HttpPost]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<ActionResult<TOutputDto>> Create([FromBody] TInputDto inputDto)
    {
        AuthenticatedUser user = _userFactory.CreateAuthenticatedUser(HttpContext);

        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        TOutputDto result = await _service.CreateAsync(inputDto, user);
        string actionName = nameof(ReadAsync).Replace("Async", "");
        var routeValues = new { id = result.Id };

        return CreatedAtAction(
            actionName,
            routeValues, 
            result);
    }

    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<ActionResult<IEnumerable<TOutputDto>>> ReadAllAsync()
    {
        AuthenticatedUser user = _userFactory.CreateAuthenticatedUser(HttpContext);
        IEnumerable<TOutputDto> result = await _service.ReadAllAsync(user);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TOutputDto>> ReadAsync(TId id)
    {
        AuthenticatedUser user = _userFactory.CreateAuthenticatedUser(HttpContext);
        TOutputDto result = await _service.ReadAsync(id, user);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TOutputDto>> UpdateAsync(TId id, [FromBody] TInputDto inputDto)
    {
        AuthenticatedUser user = _userFactory.CreateAuthenticatedUser(HttpContext);

        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        TOutputDto result = await _service.UpdateAsync(id, inputDto, user);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult> DeleteAsync(TId id)
    {
        AuthenticatedUser user = _userFactory.CreateAuthenticatedUser(HttpContext);
        await _service.DeleteAsync(id, user);
        return NoContent();
    }
}
