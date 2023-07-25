namespace SmartSpaceControl.Models;

public class Sensor
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public Room Room { get; set; }
    public double Value { get; set; }
    public SensorType SensorType { get; set; }
}