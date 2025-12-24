namespace Shared.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    bool IsAuthenticated { get; }
}

