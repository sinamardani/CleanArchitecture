using System.Security.Claims;

namespace Application.Commons.Interfaces.Authentication;

public interface IJwtService
{
    string GenerateToken(int userId);
    ClaimsPrincipal? ValidateToken(string token);
}