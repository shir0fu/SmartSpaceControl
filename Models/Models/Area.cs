namespace SmartSpaceControl.Models.Models;

public class Area
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Room>? Rooms { get; set; }
}
