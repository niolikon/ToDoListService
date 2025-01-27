using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ToDoListService.Framework.Exceptions.Rest;

namespace ToDoListService.Framework.Security.Authentication;

public static class AuthenticatedUserMapper
{
    static public AuthenticatedUser MapUser(ClaimsPrincipal user)
    {
        string id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
            throw new UnauthorizedRestException("User has no valid userId");
        string userName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ??
            throw new UnauthorizedRestException("User has no valid userName");

        return new() { Id = id, UserName = userName };
    }
    static public AuthenticatedUser MapIdentityUser(IdentityUser user)
    {
        return new() { Id = user.Id, UserName = user.UserName };
    }
}
