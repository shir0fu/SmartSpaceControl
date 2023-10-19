using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;

namespace SmartSpaceControl.Repository;

public interface IUserRepository
{
    public Task<User?> GetUserByAuthToken(string authToken);
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    public UserRepository(ApplicationDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async Task<User?> GetUserByAuthToken(string authToken)
    {
        var userToken = _dbContext.UserTokens.Where(x => x.AccessToken == authToken).First();
        return await _userManager.FindByIdAsync(userToken.UserId);
    }
}
