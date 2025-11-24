using System;

namespace PampaLeche.Domain.DomainEvents;

public record BatchCreatedEvent(Guid BatchId, string BatchCode);
