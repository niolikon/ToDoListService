using System.ComponentModel.DataAnnotations;

namespace ToDoListService.Domain.Dtos;

public class ToDoInputDto
{
    [Required]
    [MaxLength(30)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(250)]
    public required string Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateOnly DueDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(15));
}
