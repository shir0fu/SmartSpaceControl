using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SmartSpaceControl.Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartSpaceControl.Services.Helpers;

public class JwtHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("JwtConfig:Key");
        var keyBytes = Encoding.ASCII.GetBytes(key);
        return new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    }
    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static string GenerateAccessToken(IConfiguration configuration, string userName, UserManager<User> userManager, User user)
    {
        var key = configuration.GetValue<string>("JwtConfig:Key");
        var expires = configuration.GetValue<int>("JwtConfig:TokenValidityInMinutes");
        var keyBytes = Encoding.ASCII.GetBytes(key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new(ClaimTypes.Email, userName),
            new(ClaimTypes.NameIdentifier, user.Id)
            }),
            Expires = DateTime.UtcNow.AddMinutes(expires),
            SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static ClaimsPrincipal? GetPrincipalFromExpiredToken(IConfiguration configuration, string accessToken)
    {
        var tokenValidationParameters = GetTokenValidationParameters(configuration);
        tokenValidationParameters.ValidateLifetime = false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}
