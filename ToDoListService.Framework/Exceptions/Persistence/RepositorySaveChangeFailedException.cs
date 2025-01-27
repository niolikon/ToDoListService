namespace ToDoListService.Framework.Exceptions.Persistence;

public class RepositorySaveChangeFailedException(string message) : Exception(message)
{
}
