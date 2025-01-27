using System.Net;

namespace ToDoListService.Framework.Exceptions.Rest;

public class BadRequestRestException(string message) : BaseRestException(message, HttpStatusCode.BadRequest)
{
}
