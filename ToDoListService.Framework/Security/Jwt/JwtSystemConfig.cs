using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace ToDoListService.Framework.Security.Jwt;

public class JwtSystemConfig
{
    public JwtSystemConfig(IConfigurationSection configurationSection)
    {
        Secret = configurationSection["Secret"];
        Issuer = configurationSection["Issuer"];
        Audience = configurationSection["Audience"];
        SecurityAlgorithm = configurationSection["SecurityAlgorithm"];
    }

    public JwtSystemConfig() { }

    public string Secret { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string SecurityAlgorithm { get; set; }

    public JwtSystemConfig Verified()
    {
        if (string.IsNullOrEmpty(Secret))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Secret");
        }

        if (string.IsNullOrEmpty(Audience))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Audience");
        }

        if (string.IsNullOrEmpty(Issuer))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Issuer");
        }

        return this;
    }
}
