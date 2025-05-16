using System.Security.Claims;
using CleanArchitecture.Core.Application.Common.Interfaces.Data;

namespace Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    public Guid GetUserId()
    {
        var context = httpContextAccessor.HttpContext;

        if (context?.User?.Identity?.IsAuthenticated != true)
            return Guid.Empty; // کاربر لاگین نکرده، پس شناسه وجود ندارد.

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? context.User.FindFirst("sub")?.Value;

        if (Guid.TryParse(userId, out var guid))
            return guid;

        return Guid.Empty;
    }

    public Guid Id => GetUserId();
}
