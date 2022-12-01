using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/inventory/{productId", (string productId, IMemoryCache memoryCache) =>
{
    var memCacheKey = $"{productId}-inventory";
    int inventoryValue = -404;

    if (!memoryCache.TryGetValue(memCacheKey, out inventoryValue))
    {
        inventoryValue = new Random().Next(1, 100);
        memoryCache.Set(memCacheKey, inventoryValue);
    }

    inventoryValue = memoryCache.Get<int>(memCacheKey);

    return Results.Ok(inventoryValue);
})
.Produces<int>(StatusCodes.Status200OK)
.WithName("GetInventoryCount");

app.Run();