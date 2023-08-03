using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;

namespace SmartSpaceControl.Services;

public interface IRoomService
{
    public Task<IEnumerable<Room>> GetRooms(string userEmail, int areaId);
    public Task CreateRoom(Room newArea);
    public Task DeleteRoom(Room area);
    public Task EditRoom(Room newArea);
}
public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    public RoomService(ApplicationDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task CreateRoom(Room newRoom)
    {
        await _dbContext.Rooms.AddAsync(newRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRoom(Room area)
    {
        _dbContext.Rooms.Remove(area);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditRoom(Room newRoom)
    {
        _dbContext.Rooms.Update(newRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Room>> GetRooms(string userEmail, int areaId)
    {
        User? user = await _userManager.FindByEmailAsync(userEmail);

        CheckHelper.CheckNull(user);

        Area? area = _dbContext.Areas.Where(x => x.Id == areaId && x.UserId == user.Id).FirstOrDefault();

        CheckHelper.CheckNull(area, "Area not found!");

        IEnumerable<Room> rooms = _dbContext.Rooms.Where(x => x.AreaId == areaId);

        return rooms;
    }
}
