using Microsoft.AspNetCore.Identity;
using ToDoListService.Framework.Security.Authentication;

namespace ToDoListService.Framework.Security.Jwt;

public interface IJwtTokenFactory
{
    JwtTokenDto CreateJwtToken(AuthenticatedUser user, DateTime expiration);

    JwtTokenDto CreateJwtToken(IdentityUser user, DateTime expiration);
}
