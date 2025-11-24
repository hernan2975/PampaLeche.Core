using System.Threading.Tasks;

namespace PampaLeche.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}
