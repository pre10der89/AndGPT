#nullable disable

using System.Diagnostics.CodeAnalysis;
using AndGPT.Core.Contracts.Services;

namespace AndGPT.Core.Services;

public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, List<object>> _subscribers = new();

    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        if (!_subscribers.TryGetValue(typeof(TEvent), out var handlers))
        {
            handlers = [];
            _subscribers[typeof(TEvent)] = handlers;
        }

        handlers.Add(handler);
    }

    public void Publish<TEvent>([DisallowNull] TEvent eventToPublish)
    {
        if (eventToPublish == null)
        {
            throw new ArgumentNullException(nameof(eventToPublish));
        }

        if (!_subscribers.TryGetValue(eventToPublish.GetType(), out var handlers))
        {
            return;
        }

        foreach (var handler in handlers.Cast<Action<TEvent>>())
        {
            handler(eventToPublish);
        }
    }
}
