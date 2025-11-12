namespace RulesCaseApi.Models;

public sealed class VoyageSnapshot
{
    public List<PortCall> PortCalls { get; set; } = new();
}

public sealed class PortCall
{
    public string? Function { get; set; }
    public string? PortCallCode { get; set; }
    public string? Name { get; set; }
    public IList<Activity> Activities { get; set; } = new List<Activity>();
}

public sealed class Activity
{
    public string? VoyageNumber { get; set; }
    public string? VesselCode { get; set; }
    public int? PortCallSequence { get; set; }
    public string? Code { get; set; }
    public string? PortCode { get; set; }
    public string? Function { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset? Time { get; set; }
    public string? NextVoyageNumber { get; set; }
    public string? CargoShortName { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
