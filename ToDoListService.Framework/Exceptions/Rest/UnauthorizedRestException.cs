using System.Net;

namespace ToDoListService.Framework.Exceptions.Rest;

public class UnauthorizedRestException(string message) : BaseRestException(message, HttpStatusCode.Unauthorized)
{
}
