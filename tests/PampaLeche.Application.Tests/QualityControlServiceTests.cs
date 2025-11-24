using System.Threading.Tasks;
using Moq;
using PampaLeche.Application.Services;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.DomainEvents;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.Interfaces;
using PampaLeche.Domain.ValueObjects;
using Xunit;

namespace PampaLeche.Application.Tests;

public class QualityControlServiceTests
{
    [Fact]
    public async Task ProcessBatch_RejectedBatch_PublishesThresholdEvent()
    {
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new QualityControlService(mockPublisher.Object);

        var batch = MilkBatch.Create(
            "LP-TEST-004",
            DateTime.Now,
            new Temperature(12.0), // >10°C → rejected
            new FatContent(3.2),
            1.031,
            0.16,
            new MilkOrigin("LP-10045"),
            new GeoLocation(-36.6167, -64.2833),
            DestinationType.Industry
        );

        await service.ProcessBatchAsync(batch);

        mockPublisher.Verify(x => x.PublishAsync(It.IsAny<QualityThresholdBreachedEvent>()), Times.Once);
    }

    [Fact]
    public async Task ProcessBatch_AcceptedBatch_DoesNotPublishThresholdEvent()
    {
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new QualityControlService(mockPublisher.Object);

        var batch = MilkBatch.Create(
            "LP-TEST-005",
            DateTime.Now,
            new Temperature(8.0),
            new FatContent(3.2),
            1.031,
            0.16,
            new MilkOrigin("LP-10045"),
            new GeoLocation(-36.6167, -64.2833),
            DestinationType.Industry
        );

        await service.ProcessBatchAsync(batch);

        mockPublisher.Verify(x => x.PublishAsync(It.IsAny<QualityThresholdBreachedEvent>()), Times.Never);
    }
}
