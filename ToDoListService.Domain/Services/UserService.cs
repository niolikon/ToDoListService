using Microsoft.AspNetCore.Identity;
using System.Text;
using ToDoListService.Framework.Exceptions.Rest;
using ToDoListService.Framework.Security.Jwt;
using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Mappers;
using ToDoListService.Framework.Security.Authentication;

namespace ToDoListService.Domain.Services;

public class UserService : IUserService
{
    private UserManager<User> _userManager;
    private IUserMapper _userMapper;
    private IJwtTokenFactory _jwtTokenFactory;

    public UserService(UserManager<User> manager, IUserMapper mapper, IJwtTokenFactory jwtTokenFactory)
    {
        _userManager = manager;
        _userMapper = mapper;
        _jwtTokenFactory = jwtTokenFactory;
    }

    public async Task<JwtTokenDto> LoginAsync(UserLoginDto loginDto)
    {
        User? userInDb = await _userManager.FindByNameAsync(loginDto.Username);
        if (userInDb == null) 
        {
            throw new UnauthorizedRestException("User login failed");
        }

        bool passwordMatched = await _userManager.CheckPasswordAsync(userInDb, loginDto.Password);
        if (!passwordMatched)
        {
            throw new UnauthorizedRestException("User login failed");
        }

        return _jwtTokenFactory.CreateJwtToken(userInDb, DateTime.Now.AddHours(1));
    }

    public async Task<UserOutputDto> RegisterAsync(UserRegistrationDto registrationDto)
    {
        User userToCreate = _userMapper.mapToEntity(registrationDto);
        IdentityResult createResult = await _userManager.CreateAsync(userToCreate, registrationDto.PassWord);

        StringBuilder errorString = new ();
        if (! createResult.Succeeded)
        {
            foreach (IdentityError error in createResult.Errors)
            {
                errorString.Append(error.Code + " ");
            }
            
            throw new ConflictRestException($"Could not register a user with the provided data: {errorString.ToString().TrimEnd()}");
        }

        return _userMapper.mapToOutputDto(userToCreate);
    }
}
