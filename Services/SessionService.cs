using SmartSpaceControl.Services.Helpers;
using SmartSpaceControl.Models.Dto;
using SmartSpaceControl.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSpaceControl.Data;
using System.Security.Claims;


namespace SmartSpaceControl.Services;

public interface ISessionService
{
    Task<TokenDto> Authenticate(string login, string password);
    Task<TokenDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task Logout(ClaimsPrincipal user, string token);
}

public class SessionService : ISessionService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    public SessionService(IServiceScopeFactory scopeFactory, IConfiguration configuration, ApplicationDbContext dbContext, UserManager<User> userManager)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<TokenDto> Authenticate(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        user = await _userManager.FindByNameAsync(email);
        if (user == null)
        {
            throw new InvalidDataException("User not found");
        }
        var isPasswordValid = _userManager.CheckPasswordAsync(user, password).Result;

        if (!isPasswordValid)
            throw new InvalidDataException("Password is wrong");

        if (!user.EmailConfirmed)
            throw new Exception("Your email is not confirmed");

        if (await _userManager.GetLockoutEnabledAsync(user))
            throw new Exception("Can't login");

        var token = JwtHelper.GenerateAccessToken(_configuration, email, _userManager, user);
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpires = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityInDays");
        var currentUserToken = _dbContext.UserTokens.FirstOrDefault(x => x.UserId == user.Id);

        var userToken = new UserToken
        {
            UserId = user.Id,
            AccessToken = token,
            RefreshToken = refreshToken,
            CreationTime = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(refreshTokenExpires)
        };
        var model = new TokenDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken
        };

        if (currentUserToken != null)
        {
            currentUserToken.AccessToken = token;
            currentUserToken.RefreshToken = refreshToken;
            currentUserToken.Expires = DateTime.UtcNow.AddDays(refreshTokenExpires);
            currentUserToken.CreationTime = DateTime.UtcNow;
            _dbContext.Update(currentUserToken);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        _dbContext.UserTokens.Add(userToken);
        await _dbContext.SaveChangesAsync();

        return model;
    }

    public async Task<TokenDto> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var principal = JwtHelper.GetPrincipalFromExpiredToken(_configuration, refreshTokenDto.AccessToken);
        var email = principal?.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new Exception("Invalid token");
        var currentUserToken = _dbContext.UserTokens.FirstOrDefault(x => x.UserId == user.Id && x.RefreshToken == refreshTokenDto.RefreshToken);
        if (currentUserToken == null)
            throw new Exception("Invalid token");
        if (currentUserToken.Expires < DateTime.UtcNow)
            throw new Exception("Token expired");

        var newRefreshToken = JwtHelper.GenerateRefreshToken();
        var expiresInDays = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityInDays");
        var newToken = JwtHelper.GenerateAccessToken(_configuration, email, _userManager, user);
        currentUserToken.RefreshToken = newRefreshToken;
        currentUserToken.AccessToken = newToken;
        currentUserToken.Expires = DateTime.UtcNow.AddDays(expiresInDays);
        _dbContext.Update(currentUserToken);
        await _dbContext.SaveChangesAsync();
        return new TokenDto()
        {
            AccessToken = newToken,
            RefreshToken = newRefreshToken
        };

    }

    public async Task Logout(ClaimsPrincipal claims, string token)
    {
        var email = claims.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return;

        var session = await _dbContext.UserTokens.FirstOrDefaultAsync(x => x.AccessToken == token && x.UserId == user.Id);

        if (session != null)
            _dbContext.UserTokens.Remove(session);

        await _dbContext.SaveChangesAsync();
    }
}
