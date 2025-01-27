using System.Net;

namespace ToDoListService.Framework.Exceptions.Rest;

public class ConflictRestException(string message) : BaseRestException(message, HttpStatusCode.Conflict)
{
}
