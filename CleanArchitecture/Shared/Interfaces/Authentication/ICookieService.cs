namespace Shared.Interfaces.Authentication;

public interface ICookieService
{
    void SetTokenCookie(string token);
    string? GetTokenFromCookie();
    void RemoveTokenCookie();
    void SetRefreshTokenCookie(string refreshToken);
    string? GetRefreshTokenFromCookie();
    void RemoveRefreshTokenCookie();
}

