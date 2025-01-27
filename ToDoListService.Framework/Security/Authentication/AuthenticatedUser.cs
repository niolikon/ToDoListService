namespace ToDoListService.Framework.Security.Authentication;

public class AuthenticatedUser
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public override string ToString()
    {
        return $"AuthenticatedUser {{Id: {Id}, Email: {UserName}}}";
    }
}
