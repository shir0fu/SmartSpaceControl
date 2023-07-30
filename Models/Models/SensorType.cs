namespace SmartSpaceControl.Models.Models;

public class SensorType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ValType ValueType { get; set; }
    public Measure Measure { get; set; }
}

public enum ValType
{
    Temperature,
    Depth,
    Light
}