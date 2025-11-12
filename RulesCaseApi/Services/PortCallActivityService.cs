using System.Text.Json;
using RulesCaseApi.Models;

namespace RulesCaseApi.Services;

public sealed class PortCallActivityService
{
    private readonly string _dataFilePath;
    private readonly JsonSerializerOptions _serializerOptions;

    public PortCallActivityService(IConfiguration configuration, IHostEnvironment environment)
    {
        var configuredPath = configuration["PortCallData:FilePath"];

        _dataFilePath = ResolvePath(environment.ContentRootPath, configuredPath)
            ?? Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "Z388-25061.json"));

        if (!File.Exists(_dataFilePath))
        {
            throw new FileNotFoundException(
                $"JSON snapshot not found. Expected at '{_dataFilePath}'.",
                _dataFilePath);
        }

        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
    }

    public async Task<IReadOnlyList<ActivityDto>> GetActivitiesForFunctionAsync(
        string? functionCode,
        CancellationToken cancellationToken)
    {
        var normalizedFunction = string.IsNullOrWhiteSpace(functionCode)
            ? "L"
            : functionCode.Trim();

        await using var stream = File.OpenRead(_dataFilePath);
        var snapshot = await JsonSerializer.DeserializeAsync<VoyageSnapshot>(
            stream,
            _serializerOptions,
            cancellationToken) ?? throw new InvalidOperationException("Failed to parse voyage snapshot.");

        var activities = snapshot.PortCalls
            .Where(pc => string.Equals(pc.Function, normalizedFunction, StringComparison.OrdinalIgnoreCase))
            .SelectMany(pc => pc.Activities.Select(activity => (PortCall: pc, Activity: activity)))
            .OrderByDescending(tuple => tuple.Activity.Time)
            .Select(tuple => new ActivityDto(
                tuple.PortCall.PortCallCode,
                tuple.PortCall.Name,
                tuple.Activity.VoyageNumber,
                tuple.Activity.PortCallSequence,
                tuple.Activity.Name,
                tuple.Activity.Time,
                tuple.Activity.CargoShortName,
                tuple.Activity.UpdatedBy,
                tuple.Activity.UpdatedAt))
            .ToList();

        return activities;
    }

    private static string? ResolvePath(string contentRoot, string? configuredPath)
    {
        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            return null;
        }

        return Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.GetFullPath(Path.Combine(contentRoot, configuredPath));
    }
}
