namespace SmartSpaceControl.Models.Models;

public class Sensor
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int RoomId { get; set; }
    public string Name { get; set; }
    public double Value { get; set; }
    public SensorType SensorType { get; set; }
}