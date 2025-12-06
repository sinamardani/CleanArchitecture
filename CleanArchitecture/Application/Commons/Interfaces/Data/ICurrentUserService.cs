namespace Application.Commons.Interfaces.Data;

public interface ICurrentUserService
{
    int? UserId { get; }
    bool IsAuthenticated { get; }
}

