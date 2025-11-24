using System.Threading.Tasks;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Interfaces;

namespace PampaLeche.Application.Services;

public class QualityControlService
{
    private readonly IEventPublisher _eventPublisher;

    public QualityControlService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task ProcessBatchAsync(MilkBatch batch)
    {
        if (batch.Status == QualityStatus.Rejected)
        {
            await _eventPublisher.PublishAsync(new DomainEvents.QualityThresholdBreachedEvent(batch.Id));
        }
    }
}
