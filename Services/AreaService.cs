using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Dto;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;
using System.Security.Claims;


namespace SmartSpaceControl.Services;

public interface IAreaService
{
    public Task CreateArea(AreaDto newArea, User user);
    public Task<IEnumerable<Area>> GetAreas(User user);
    public Task UpdateArea(Area newArea);
    public Task DeleteArea(User user, int id);
    public Task<Area> GetAreaById(User user, int id);
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

    public async Task CreateArea(AreaDto newArea, User user)
    {
        Area area = new Area()
        {
            Name = newArea.Name,
            Description = newArea.Description,
            UserId = user.Id
        };
        await _dbContext.Areas.AddAsync(area);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Area>> GetAreas(User user)
    {
        IEnumerable<Area> areas = _dbContext.Areas.Where(x => x.UserId == user.Id);
        return areas;
    }

    public async Task UpdateArea(Area newArea)
    {
        _dbContext.Areas.Update(newArea);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteArea(User user, int id)
    {
        var area = await GetAreaById(user, id);
        _dbContext.Areas.Remove(area);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Area> GetAreaById(User user, int id)
    {
        var area = await _dbContext.Areas.Where(x => x.Id == id && x.UserId == user.Id).FirstOrDefaultAsync();
        CheckHelper.CheckNull(area, "Area not found");

        return area;
    }

}
