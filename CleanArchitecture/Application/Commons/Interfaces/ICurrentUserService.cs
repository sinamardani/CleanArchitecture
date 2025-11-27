namespace Application.Commons.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    bool IsAuthenticated { get; }
}

