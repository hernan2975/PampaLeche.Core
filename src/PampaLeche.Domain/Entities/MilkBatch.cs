using System;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Domain.Entities;

public class MilkBatch
{
    public Guid Id { get; private set; }
    public string BatchCode { get; private set; }
    public DateTime CollectionTime { get; private set; }
    public DateTime? CoolingTime { get; private set; }
    public Temperature InitialTemp { get; private set; }
    public Temperature? StorageTemp { get; private set; }
    public FatContent Fat { get; private set; }
    public double Density { get; private set; }
    public double Acidity { get; private set; }
    public MilkOrigin Origin { get; private set; }
    public GeoLocation FarmLocation { get; private set; }
    public DestinationType Destination { get; private set; }
    public QualityStatus Status { get; private set; }

    private MilkBatch() { }

    public static MilkBatch Create(
        string batchCode,
        DateTime collectionTime,
        Temperature initialTemp,
        FatContent fat,
        double density,
        double acidity,
        MilkOrigin origin,
        GeoLocation farmLocation,
        DestinationType destination)
    {
        var batch = new MilkBatch
        {
            Id = Guid.NewGuid(),
            BatchCode = batchCode,
            CollectionTime = collectionTime,
            InitialTemp = initialTemp,
            Fat = fat,
            Density = density,
            Acidity = acidity,
            Origin = origin,
            FarmLocation = farmLocation,
            Destination = destination
        };

        batch.Status = batch.DetermineQualityStatus();
        return batch;
    }

    public void RegisterCooling(DateTime coolingTime, Temperature storageTemp)
    {
        if (coolingTime < CollectionTime)
            throw new InvalidOperationException("Cooling time cannot precede collection.");

        CoolingTime = coolingTime;
        StorageTemp = storageTemp;

        Status = DetermineQualityStatus();
    }

    private QualityStatus DetermineQualityStatus()
    {
        if (InitialTemp.Value > 10) return QualityStatus.Rejected;
        if (Fat.Value < 3.0) return QualityStatus.Rejected;
        if (Acidity > 0.18) return QualityStatus.Rejected;
        if (Density is < 1.028 or > 1.034) return QualityStatus.Warning;
        if (CoolingTime.HasValue && (CoolingTime.Value - CollectionTime).TotalMinutes > 120)
            return QualityStatus.Warning;
        return QualityStatus.Accepted;
    }
}
