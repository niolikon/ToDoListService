namespace ToDoListService.Framework.Exceptions.Persistence;

public class EntityOwnershipViolationException(string message) : Exception(message)
{
}
