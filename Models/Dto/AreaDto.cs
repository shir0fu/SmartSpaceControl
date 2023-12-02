using SmartSpaceControl.Models.Models;

namespace SmartSpaceControl.Models.Dto;

public class AreaDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Room>? Rooms { get; set; }
}
