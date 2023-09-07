using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Dto;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;
using System.Security.Claims;


namespace SmartSpaceControl.Services;

public interface IAreaService
{
    public Task CreateArea(AreaDto newArea);
    public Task<IEnumerable<Area>> GetAreas();
    public Task UpdateArea(Area newArea);
    public Task DeleteArea(Area area);
    
}

public class AreaService : IAreaService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AreaService(ApplicationDbContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task CreateArea(AreaDto newArea)
    {
        var currentUser = await _userManager.GetUserAsync(_signInManager.Context.User);
        CheckHelper.CheckNull(currentUser);

        Area area = new Area()
        {
            Name = newArea.Name,
            Description = newArea.Description,
            UserId = currentUser.Id
        };
        await _dbContext.Areas.AddAsync(area);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Area>> GetAreas()
    {
        var currentUser = await _userManager.GetUserAsync(_signInManager.Context.User);
        CheckHelper.CheckNull(currentUser);

        IEnumerable<Area> areas = _dbContext.Areas.Where(x => x.UserId == currentUser.Id);
        return areas;
    }

    public async Task UpdateArea(Area newArea)
    {
        _dbContext.Areas.Update(newArea);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteArea(Area area)
    {
        _dbContext.Areas.Remove(area);
        await _dbContext.SaveChangesAsync();
    }

}
