using System.Security.Cryptography;
using NextHome.Core.Interfaces;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using NextHome.Infrastructure.Options;


namespace NextHome.Infrastructure.Services;

/// <inheritdoc/>
public class TokenGenerator(IOptions<JwtOptions> jwtOptions) : ITokenGenerator
{
    /// <inheritdoc/>
    public string GenerateToken(int length = 32) => 
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));

    /// <inheritdoc/>
    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
    
    /// <inheritdoc/>
    public string GenerateJwtToken(Guid userId, string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials:
            new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}