#nullable disable

namespace AndGPT.Core.Contracts.Services;

public interface IEventAggregator
{
    void Subscribe<TEvent>(Action<TEvent> handler);
    void Publish<TEvent>(TEvent eventToPublish);
}
