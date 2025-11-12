namespace RulesCaseApi.Models;

public sealed record ActivityDto(
    string? PortCallCode,
    string? PortName,
    string? VoyageNumber,
    int? PortCallSequence,
    string? ActivityName,
    DateTimeOffset? Time,
    string? CargoShortName,
    string? UpdatedBy,
    DateTimeOffset? UpdatedAt);
