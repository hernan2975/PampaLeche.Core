using Microsoft.EntityFrameworkCore;
using PampaLeche.Application.Services;
using PampaLeche.Infrastructure.Messaging;
using PampaLeche.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite("Data Source=data/batches.db"));
builder.Services.AddScoped<QualityControlService>();
builder.Services.AddSingleton<IEventPublisher, LocalEventBus>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "PampaLeche API - La Pampa");

app.MapPost("/batches", async (MilkBatchDto dto, QualityControlService qc, ApplicationDbContext db) =>
{
    var batch = MilkBatch.Create(
        dto.BatchCode,
        dto.CollectionTime,
        new Temperature(dto.InitialTemp),
        new FatContent(dto.Fat),
        dto.Density,
        dto.Acidity,
        new MilkOrigin(dto.ProducerCode),
        new GeoLocation(dto.Latitude, dto.Longitude),
        (DestinationType)dto.Destination
    );

    if (dto.CoolingTime.HasValue)
        batch.RegisterCooling(dto.CoolingTime.Value, new Temperature(dto.StorageTemp));

    await qc.ProcessBatchAsync(batch);
    db.Batches.Add(batch);
    await db.SaveChangesAsync();

    return Results.Created($"/batches/{batch.Id}", new { batch.Id, batch.BatchCode, batch.Status });
});

app.Run();

public record MilkBatchDto(
    string BatchCode,
    DateTime CollectionTime,
    double InitialTemp,
    double Fat,
    double Density,
    double Acidity,
    string ProducerCode,
    double Latitude,
    double Longitude,
    int Destination,
    DateTime? CoolingTime = null,
    double? StorageTemp = null
);
