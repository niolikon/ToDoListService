using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;

namespace ToDoListService.Domain.Mappers;

public class UserMapper : IUserMapper
{
    public User mapToEntity(UserRegistrationDto dto)
    {
        return new User()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Email = dto.Email,
            UserName = dto.UserName
        };
    }

    public UserOutputDto mapToOutputDto(User entity)
    {
        return new UserOutputDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            Email = entity.Email,
            UserName = entity.UserName
        };
    }
}
