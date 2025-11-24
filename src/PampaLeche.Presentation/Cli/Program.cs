using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PampaLeche.Application.Services;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;
using PampaLeche.Infrastructure.Messaging;
using PampaLeche.Infrastructure.Sensors;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ISensorAdapter, MockTankSensor>();
        services.AddSingleton<IEventPublisher, LocalEventBus>();
        services.AddSingleton<QualityControlService>();
    })
    .Build();

var qc = host.Services.GetRequiredService<QualityControlService>();

Console.WriteLine("🥛 PampaLeche.Core — Control de Calidad Láctea (La Pampa)");
Console.WriteLine("Modo: Producción — Offline-First");

var batch = MilkBatch.Create(
    "LP2025-11-24-001",
    DateTime.Now.AddHours(-1),
    new Temperature(8.5),
    new FatContent(3.2),
    1.031,
    0.16,
    new MilkOrigin("LP-10045"),
    new GeoLocation(-36.6167, -64.2833),
    DestinationType.Industry
);

batch.RegisterCooling(DateTime.Now, new Temperature(3.8));

await qc.ProcessBatchAsync(batch);

Console.WriteLine($"✅ Lote {batch.BatchCode} procesado.");
Console.WriteLine($"Estado: {batch.Status}");
