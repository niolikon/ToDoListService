using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoListService.Framework.Security.Authentication;

namespace ToDoListService.Framework.Security.Jwt;

public class JwtTokenFactory : IJwtTokenFactory
{
    private JwtSystemConfig _config;

    public JwtTokenFactory(JwtSystemConfig config)
    {
        _config = config;
    }

    public JwtTokenDto CreateJwtToken(AuthenticatedUser user, DateTime expiration)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config.Secret));
        SigningCredentials credentials = new(key, _config.SecurityAlgorithm);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtTokenDto() { Token = tokenAsString };
    }

    public JwtTokenDto CreateJwtToken(IdentityUser user, DateTime expiration)
    {
        AuthenticatedUser authenticatedUser = AuthenticatedUserMapper.MapIdentityUser(user);
        return CreateJwtToken(authenticatedUser, expiration);
    }
}
