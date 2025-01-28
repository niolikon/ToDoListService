using ToDoListService.Framework.Dtos;

namespace ToDoListService.Domain.Dtos;

public class ToDoOutputDto : BaseOutputDto<int>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public bool? IsCompleted { get; set; }

    public DateOnly? DueDate { get; set; }
}
