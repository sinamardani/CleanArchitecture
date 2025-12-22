using Application.Commons.Interfaces.Authentication;

namespace Web.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : ICookieService
{
    private const string TokenCookieName = "access_token";
    private const string RefreshTokenCookieName = "refresh_token";

    public void SetTokenCookie(string token)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = configuration.GetValue("CookieSettings:Secure", true),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddMinutes(configuration.GetValue("JwtSettings:ExpirationMinutes", 60))
        };

        httpContext.Response.Cookies.Append(TokenCookieName, token, cookieOptions);
    }

    public string? GetTokenFromCookie()
    {
        var httpContext = httpContextAccessor.HttpContext;
        return httpContext?.Request.Cookies[TokenCookieName];
    }

    public void RemoveTokenCookie()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        httpContext.Response.Cookies.Delete(TokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = configuration.GetValue("CookieSettings:Secure", true),
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });
    }

    public void SetRefreshTokenCookie(string refreshToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = configuration.GetValue("CookieSettings:Secure", true),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(configuration.GetValue("CookieSettings:RefreshTokenExpirationDays", 7))
        };

        httpContext.Response.Cookies.Append(RefreshTokenCookieName, refreshToken, cookieOptions);
    }

    public string? GetRefreshTokenFromCookie()
    {
        var httpContext = httpContextAccessor.HttpContext;
        return httpContext?.Request.Cookies[RefreshTokenCookieName];
    }

    public void RemoveRefreshTokenCookie()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        httpContext.Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = configuration.GetValue("CookieSettings:Secure", true),
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });
    }
}

