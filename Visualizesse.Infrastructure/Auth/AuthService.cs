using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Services;

namespace Visualizesse.Infrastructure.Auth;

public class AuthService(IConfiguration configuration) : IAuthService
{
    private readonly DateTime expiresAt = DateTime.Now.AddMinutes(30);
    public string GenerateJWTToken(User user)
    {
        var key = configuration["JWT:Key"];
        var issuer = configuration["JWT:Issuer"];
        var audience = configuration["JWT:Audience"];
        
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("id", user.Uuid.ToString()),
            new Claim("name", user.Name)
        };
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: expiresAt,
            signingCredentials: credentials,
            claims: claims
        );

        var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

        return stringToken;
    }

    public string ComputeSHA256Hash(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public IEnumerable<Claim> ReadJWTToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        return jwtToken.Claims;
    }

    public bool CompareComputedSHA256Hash(string password, string hashedPassword)
    {
        return hashedPassword == ComputeSHA256Hash(password);
    }
}