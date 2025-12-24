using System.Security.Claims;
using Shared.Interfaces;

namespace Web.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public int? UserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value
                ?? httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;

            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }
    }

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}

