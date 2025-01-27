namespace ToDoListService.Framework.Entities;

public abstract class BaseOwnedEntity<TId, TUser>
{
    public TId Id { get; set; }

    public TUser Owner { get; set; }

    public abstract void CopyFrom(BaseOwnedEntity<TId, TUser> other);
}
