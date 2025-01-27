namespace ToDoListService.Framework.Exceptions.Persistence;

public class EntityNotFoundException(string message) : Exception(message)
{
}
