namespace VoyageApi.Models;

public class Activity
{
    public string? Name { get; set; }
    public string? Time { get; set; }
}

public class PortCall
{
    public string? Function { get; set; }
    public string? Name { get; set; }
    public List<Activity>? Activities { get; set; }
}

public class VoyageData
{
    public List<PortCall>? PortCalls { get; set; }
}