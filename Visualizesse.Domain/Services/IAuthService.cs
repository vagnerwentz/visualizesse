using System.Security.Claims;
using Visualizesse.Domain.Entities;

namespace Visualizesse.Domain.Services;

public interface IAuthService
{
    string GenerateJWTToken(User user);
    string ComputeSHA256Hash(string password);
    bool CompareComputedSHA256Hash(string password, string hashedPassword);
    IEnumerable<Claim> ReadJWTToken(string token);
}