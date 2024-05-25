using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Microsoft.Windows.AppLifecycle;
using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Events;

namespace HeyGPT.App.Activation;

public sealed class ProtocolActivationHandler : ActivationHandler<AppActivationArguments>
{
    private const string HandlerName = "Protocol";
    private const string HeyProtocol = "hey";
    private const string GptProtocol = "gpt";

    private readonly IEventAggregator _eventAggregator;

    public ProtocolActivationHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    }

    public override string Name => HandlerName;

    protected override bool CanHandleInternal(AppActivationArguments args)
    {
        return args.Kind == ExtendedActivationKind.Protocol;
    }

    protected async override Task HandleInternalAsync(AppActivationArguments args)
    {
        if (args.Data is IProtocolActivatedEventArgs protocolActivatedEventArgs)
        {
            var protocol = GetProtocolScheme(protocolActivatedEventArgs);

            Debug.WriteLine($"The URI protocol[{protocol}] was received.");

            switch (protocol)
            {
                case HeyProtocol:
                    await HandleHeyProtocol(protocolActivatedEventArgs);
                    break;
                case GptProtocol:
                    await HandleGptProtocol(protocolActivatedEventArgs);
                    break;
                default:
                    Debug.WriteLine("The URI protocol was not recognized - Ignoring.");
                    break;
            }
        }
    }

    private static string GetProtocolScheme(IProtocolActivatedEventArgs args)
    {
        return args.Uri?.Scheme is not null ? args.Uri.Scheme.ToLowerInvariant() : string.Empty;
    }

    private Task HandleHeyProtocol(IProtocolActivatedEventArgs args)
    {
        // Warning: Protocols may contain sensitive information!!
       _eventAggregator.Publish(new ProtocolActivatedEvent(HeyProtocol, args.Uri));

       return Task.CompletedTask;
    }

    private Task HandleGptProtocol(IProtocolActivatedEventArgs args)
    {
        // Warning: Protocols may contain sensitive information!!
        _eventAggregator.Publish(new ProtocolActivatedEvent(GptProtocol, args.Uri));

        return Task.CompletedTask;
    }
}
