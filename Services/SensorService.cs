using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Data;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;

namespace SmartSpaceControl.Services;

public interface ISensorService
{
    public Task AddSensor(Sensor sensor);
    public Task<IEnumerable<Sensor>> GetSensor(string userEmail, int roomId);
    public Task UpdateSensor(Sensor sensor);
    public Task RemoveSensor(Sensor sensor);
    
}

public class SensorService : ISensorService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    public SensorService(ApplicationDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async Task AddSensor(Sensor sensor)
    {
        await _dbContext.Sensors.AddAsync(sensor);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Sensor>> GetSensor(string userEmail, int roomId)
    {
        User? user = await _userManager.FindByEmailAsync(userEmail);

        CheckHelper.CheckNull(user);
        
        IEnumerable<Sensor> sensors = _dbContext.Sensors.Where(x => x.UserId == user.Id && x.RoomId == roomId);

        return sensors;
    }

    public async Task UpdateSensor(Sensor sensor)
    {
        _dbContext.Sensors.Update(sensor);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveSensor(Sensor sensor)
    {
        _dbContext.Sensors.Remove(sensor);
        await _dbContext.SaveChangesAsync();
    }

}
