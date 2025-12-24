using System.Security.Claims;

namespace Shared.Interfaces.Authentication;

public interface IJwtService
{
    string GenerateToken(int userId);
    ClaimsPrincipal? ValidateToken(string token);
}

