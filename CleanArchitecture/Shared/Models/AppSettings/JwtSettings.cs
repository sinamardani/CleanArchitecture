namespace Shared.Models.AppSettings;

public class JwtSettings
{
    public string PrivateKeyPath { get; set; } = string.Empty;
    public string PublicKeyPath { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}

