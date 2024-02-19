#region

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Services;

public sealed class TokenService
{
    private readonly string _audience;
    private readonly IHostEnvironment _environment;
    private readonly string _issuer;
    private readonly string _key;

    public TokenService(IConfiguration configuration, IHostEnvironment environment)
    {
        _environment = environment;
        _issuer = configuration["Auth:Issuer"]!;
        _audience = configuration["Auth:Audience"]!;
        _key = configuration["Auth:Key"]!;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString())
        };

        var jwt = new JwtSecurityToken(
                                       _issuer,
                                       _audience,
                                       claims,
                                       expires: DateTime.UtcNow.Add(TimeSpan.FromDays(_environment.IsDevelopment()
                                                                                     ? 365
                                                                                     : 7)),
                                       signingCredentials:
                                       new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                                                              SecurityAlgorithms.HmacSha256)
                                      );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
