using RulesCaseApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<PortCallActivityService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/portcalls/activities", async (
    string? function,
    PortCallActivityService service,
    CancellationToken cancellationToken) =>
{
    var activities = await service.GetActivitiesForFunctionAsync(function, cancellationToken);

    if (activities.Count == 0)
    {
        return Results.NotFound(new
        {
            message = $"No activities found for function '{function ?? "L"}'."
        });
    }

    return Results.Ok(new
    {
        function = string.IsNullOrWhiteSpace(function) ? "L" : function.Trim().ToUpperInvariant(),
        count = activities.Count,
        activities
    });
})
.WithName("GetPortCallActivities")
.WithOpenApi();

app.Run();
