using System;

namespace PampaLeche.Domain.Entities;

public record SensorReading(
    Guid SensorId,
    string SensorType, // "TankTemp", "AgitatorRPM", etc.
    double Value,
    string Unit,
    DateTime Timestamp
);
