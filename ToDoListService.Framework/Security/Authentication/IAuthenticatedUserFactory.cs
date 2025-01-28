using Microsoft.AspNetCore.Http;

namespace ToDoListService.Framework.Security.Authentication;

public interface IAuthenticatedUserFactory
{
    AuthenticatedUser BuildAuthenticatedUser(HttpContext context);
}
