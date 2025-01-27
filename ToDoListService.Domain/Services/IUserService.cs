using ToDoListService.Framework.Security.Jwt;
using ToDoListService.Domain.Dtos;

namespace ToDoListService.Domain.Services;

public interface IUserService
{
    Task<JwtTokenDto> LoginAsync(UserLoginDto loginDto);

    Task<UserOutputDto> RegisterAsync(UserRegistrationDto userDto);
}
