using System;
using PampaLeche.Domain.Enums;
using PampaLeche.Domain.ValueObjects;

namespace PampaLeche.Domain.Entities;

public record QualityCertificate(
    Guid BatchId,
    string BatchCode,
    DateTime IssuedAt,
    string IssuedBy, // "Técnico LP-001", "Sistema Automático"
    QualityStatus Status,
    double Fat,
    double Density,
    double Acidity,
    Temperature StorageTemp,
    bool IsSenasaCompliant
);
