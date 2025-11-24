using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PampaLeche.Application.Services;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;
using PampaLeche.Infrastructure.Messaging;
using PampaLeche.Infrastructure.Sensors;
using Xunit;

namespace PampaLeche.IntegrationTests;

public class EndToEndTests
{
    [Fact]
    public async Task CliSimulation_CompletesWithoutError()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<ISensorAdapter, MockTankSensor>();
                services.AddSingleton<IEventPublisher, LocalEventBus>();
                services.AddSingleton<QualityControlService>();
            })
            .Build();

        var qc = host.Services.GetRequiredService<QualityControlService>();

        var batch = MilkBatch.Create(
            "LP-INTEG-001",
            DateTime.Now.AddHours(-1),
            new Temperature(9.0),
            new FatContent(3.1),
            1.030,
            0.17,
            new MilkOrigin("LP-10045"),
            new GeoLocation(-36.6167, -64.2833),
            DestinationType.Industry
        );

        batch.RegisterCooling(DateTime.Now, new Temperature(3.9));

        await qc.ProcessBatchAsync(batch);

        Assert.Equal(QualityStatus.Accepted, batch.Status);
    }
}
