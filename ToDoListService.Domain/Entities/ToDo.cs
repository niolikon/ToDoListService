using System.ComponentModel.DataAnnotations;
using ToDoListService.Framework.Entities;

namespace ToDoListService.Domain.Entities;

public class ToDo : BaseOwnedEntity<int, User>
{
    [MaxLength(30)]
    public string Title { get; set; }

    [MaxLength(250)]
    public string Description { get; set; }

    public bool? IsCompleted { get; set; } = null;

    public DateOnly? DueDate { get; set; } = null;

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
            if (t.IsCompleted != null)
            {
                this.IsCompleted = t.IsCompleted;
            }
            if (t.DueDate != null)
            {
                this.DueDate = t.DueDate;
            }
        }
    }
}
