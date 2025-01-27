namespace ToDoListService.Domain.Dtos;

public class ToDoPatchDto
{
    public bool? IsCompleted { get; set; } = null;

    public DateOnly? DueDate { get; set; } = null;
}
