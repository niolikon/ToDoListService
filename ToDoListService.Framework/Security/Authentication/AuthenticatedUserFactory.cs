using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ToDoListService.Framework.Exceptions.Rest;

namespace ToDoListService.Framework.Security.Authentication;

public class AuthenticatedUserFactory : IAuthenticatedUserFactory
{
    public AuthenticatedUser CreateAuthenticatedUser(HttpContext context)
    {
        ClaimsPrincipal principal = context.User;
        if (principal.Identity == null || !principal.Identity.IsAuthenticated)
        {
            throw new UnauthorizedRestException("Error in user authentication");
        }

        return AuthenticatedUserMapper.MapUser(context.User);
    }
}

