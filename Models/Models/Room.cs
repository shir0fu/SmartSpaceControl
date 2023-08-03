namespace SmartSpaceControl.Models.Models;

public class Room
{
    public int Id { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? AreaId { get; set; }
    public List<Sensor> Sensors { get; set; }
}