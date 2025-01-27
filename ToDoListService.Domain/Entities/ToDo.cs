using System.ComponentModel.DataAnnotations;
using ToDoListService.Framework.Entities;

namespace ToDoListService.Domain.Entities;

public class ToDo : BaseOwnedEntity<int, User>
{
    [Required]
    [MaxLength(30)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(250)]
    public required string Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateOnly? DueDate { get; set; } = null;

    public string UserId { get; set; }

    public override void CopyFrom(BaseOwnedEntity<int, User> other)
    {
        if (other is ToDo t)
        {
            if (!string.IsNullOrEmpty(t.Title))
            {
                this.Title = t.Title;
            }
            if (!string.IsNullOrEmpty(t.Description))
            {
                this.Description = t.Description;
            }
            if (t.DueDate != null)
            {
                this.DueDate = t.DueDate;
            }
            this.IsCompleted = t.IsCompleted;
        }
    }
}
