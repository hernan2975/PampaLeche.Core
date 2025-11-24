using System;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;
using Xunit;

namespace PampaLeche.Domain.Tests;

public class MilkBatchTests
{
    [Fact]
    public void Create_ValidData_ReturnsBatchWithAcceptedStatus()
    {
        var batch = MilkBatch.Create(
            "LP-TEST-001",
            DateTime.Now,
            new Temperature(8.0),
            new FatContent(3.2),
            1.031,
            0.16,
            new MilkOrigin("LP-10045"),
            new GeoLocation(-36.6167, -64.2833),
            DestinationType.Industry
        );

        Assert.Equal(QualityStatus.Accepted, batch.Status);
    }

    [Fact]
    public void Create_LowFat_ReturnsRejectedStatus()
    {
        var batch = MilkBatch.Create(
            "LP-TEST-002",
            DateTime.Now,
            new Temperature(8.0),
            new FatContent(2.8), // < 3.0
            1.031,
            0.16,
            new MilkOrigin("LP-10045"),
            new GeoLocation(-36.6167, -64.2833),
            DestinationType.Industry
        );

        Assert.Equal(QualityStatus.Rejected, batch.Status);
    }

    [Fact]
    public void RegisterCooling_DelayedCooling_SetsWarningStatus()
    {
        var batch = MilkBatch.Create(
            "LP-TEST-003",
            DateTime.Now.AddHours(-3),
            new Temperature(8.0),
            new FatContent(3.2),
            1.031,
            0.16,
            new MilkOrigin("LP-10045"),
            new GeoLocation(-36.6167, -64.2833),
            DestinationType.Industry
        );

        batch.RegisterCooling(DateTime.Now, new Temperature(4.0));

        Assert.Equal(QualityStatus.Warning, batch.Status);
    }
}
