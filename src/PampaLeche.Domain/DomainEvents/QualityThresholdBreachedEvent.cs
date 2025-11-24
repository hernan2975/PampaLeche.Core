using System;

namespace PampaLeche.Domain.DomainEvents;

public record QualityThresholdBreachedEvent(Guid BatchId);
