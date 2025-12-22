using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Commons.Interfaces.Authentication;
using Application.Commons.Models.AppSettings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ECDsa _privateKey;
    private readonly ECDsa _publicKey;

    public JwtService(IOptions<JwtSettings> jwtSettings, IHostEnvironment environment)
    {
        _jwtSettings = jwtSettings.Value;
        
        var basePath = environment.ContentRootPath;
        var privateKeyPath = Path.Combine(basePath, _jwtSettings.PrivateKeyPath);
        var publicKeyPath = Path.Combine(basePath, _jwtSettings.PublicKeyPath);

        if (!File.Exists(privateKeyPath))
            throw new FileNotFoundException($"Private key file not found: {privateKeyPath}");
        
        if (!File.Exists(publicKeyPath))
            throw new FileNotFoundException($"Public key file not found: {publicKeyPath}");

        var privateKeyContent = File.ReadAllText(privateKeyPath);
        var publicKeyContent = File.ReadAllText(publicKeyPath);

        _privateKey = ECDsa.Create();
        _privateKey.ImportFromPem(privateKeyContent);
        
        _publicKey = ECDsa.Create();
        _publicKey.ImportFromPem(publicKeyContent);
    }

    public string GenerateToken(int userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var signingCredentials = new SigningCredentials(
            new ECDsaSecurityKey(_privateKey),
            SecurityAlgorithms.EcdsaSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new ECDsaSecurityKey(_publicKey),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            return null;
        }
    }
}