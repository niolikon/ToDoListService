using Microsoft.AspNetCore.Mvc;
using ToDoListService.Framework.Exceptions.Rest;
using ToDoListService.Framework.Security.Jwt;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Services;

namespace ToDoListService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto login)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        JwtTokenDto result = await _service.LoginAsync(login);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationDto userData)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        UserOutputDto result = await _service.RegisterAsync(userData);
        return Ok(result);
    }
}
