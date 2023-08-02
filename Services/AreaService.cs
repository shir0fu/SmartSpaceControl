using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;

namespace SmartSpaceControl.Services;

public interface IAreaService
{
    public Task CreateArea(Area newArea);
    public Task DeleteArea(Area area);
    public Task EditArea(Area newArea);
    public Task<IEnumerable<Area>> GetAreas(string userEmail);

}

public class AreaService : IAreaService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    public AreaService(ApplicationDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async Task CreateArea(Area newArea)
    {
        await _dbContext.Areas.AddAsync(newArea);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteArea(Area area)
    {
        _dbContext.Areas.Remove(area);
        await _dbContext.SaveChangesAsync();
    }
    public async Task EditArea(Area newArea)
    {
        _dbContext.Areas.Update(newArea);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Area>> GetAreas(string userEmail)
    {
        User? user = await _userManager.FindByEmailAsync(userEmail);

        CheckHelper.CheckNull(user);

        IEnumerable<Area> areas = _dbContext.Areas.Where(x => x.UserId == user.Id);
        return areas;
    }
}
