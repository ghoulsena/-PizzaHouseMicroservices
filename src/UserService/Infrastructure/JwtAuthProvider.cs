using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtOptioncs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure;

public class JwtAuthProvider : IAuthProvider
{
    private readonly JwtOptions _options;

    public JwtAuthProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;


    }
    public string GenerateToken(User user)
    {

        var tokenhandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.Key);
      
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        });

        var token = tokenhandler.CreateJwtSecurityToken(issuer: _options.Issuer,
             audience: _options.Audience,
             subject: claimsIdentity,
             expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
             issuedAt: DateTime.Now,
             signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
             );
        return tokenhandler.WriteToken(token);
        
    }
    
}