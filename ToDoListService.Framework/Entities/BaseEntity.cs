namespace ToDoListService.Framework.Entities;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; }

    public abstract void CopyFrom(BaseEntity<TId> other);
}
