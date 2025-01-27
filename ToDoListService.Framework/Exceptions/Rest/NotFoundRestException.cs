using System.Net;

namespace ToDoListService.Framework.Exceptions.Rest;

public class NotFoundRestException(string message) : BaseRestException(message, HttpStatusCode.NotFound)
{
}
