using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;

namespace SmartSpaceControl.Services;

public interface IRoomService
{
    public Task CreateRoom(Room newArea);
    public Task<IEnumerable<Room>> GetRooms(string userEmail, int areaId);
    public Task UpdateRoom(Room newArea);
    public Task DeleteRoom(Room area);
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

    public async Task<IEnumerable<Room>> GetRooms(string userEmail, int areaId)
    {
        User? user = await _userManager.FindByEmailAsync(userEmail);

        CheckHelper.CheckNull(user);

        IEnumerable<Room> rooms = _dbContext.Rooms.Where(x => x.AreaId == areaId);

        return rooms;
    }

    public async Task UpdateRoom(Room newRoom)
    {
        _dbContext.Rooms.Update(newRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRoom(Room area)
    {
        _dbContext.Rooms.Remove(area);
        await _dbContext.SaveChangesAsync();
    }

}
