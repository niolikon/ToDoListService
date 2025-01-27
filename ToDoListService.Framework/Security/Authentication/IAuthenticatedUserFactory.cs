using Microsoft.AspNetCore.Http;

namespace ToDoListService.Framework.Security.Authentication;

public interface IAuthenticatedUserFactory
{
    AuthenticatedUser CreateAuthenticatedUser(HttpContext context);
}
