using Microsoft.IdentityModel.Tokens;
using Profiler.MinimalAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Profiler.MinimalAPI.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        // List of Claims.
        List<Claim> claims = new()
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role!.RoleName)
        };

        // Create Key.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSettings:Key")!));

        // Signing Credentials.
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Generate Token.
        var token = new JwtSecurityToken(
                _config.GetValue<string>("JwtSettings:Issuer"),
                _config.GetValue<string>("JwtSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

        // Write the Token.
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        // Return it.
        return jwt;
    }
}
