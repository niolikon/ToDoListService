using ToDoListService.Domain.Dtos;
using ToDoListService.Domain.Entities;

namespace ToDoListService.Domain.Mappers;

public class ToDoMapper : IToDoMapper
{
    public ToDo mapToEntity(ToDoInputDto dto)
    {
        return new ToDo() { 
            Description = dto.Description, 
            Title = dto.Title, 
            DueDate = dto.DueDate, 
            IsCompleted = dto.IsCompleted
        };
    }

    public ToDoOutputDto mapToOutputDto(ToDo entity)
    {
        ToDoOutputDto result = new ()
        {
            Id = entity.Id,
            Description = entity.Description,
            Title = entity.Title,
            DueDate = entity.DueDate,
            IsCompleted = entity.IsCompleted
        };

        return result;
    }
}
