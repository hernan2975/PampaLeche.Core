using System;
using System.Threading.Tasks;
using PampaLeche.Domain.Interfaces;

namespace PampaLeche.Infrastructure.Messaging;

public class LocalEventBus : IEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        Console.WriteLine($"[EVENT] {@event.GetType().Name}");
        return Task.CompletedTask;
    }
}
