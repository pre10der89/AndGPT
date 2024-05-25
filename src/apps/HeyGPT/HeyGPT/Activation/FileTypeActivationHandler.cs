using Windows.ApplicationModel.Activation;
using Microsoft.Windows.AppLifecycle;
using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Events;
using AndGPT.Core.Models;

namespace HeyGPT.App.Activation;

public sealed class FileTypeActivationHandler : ActivationHandler<AppActivationArguments>
{
    private const string HandlerName = "File";

    private readonly IEventAggregator _eventAggregator;

    public FileTypeActivationHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    }

    public override string Name => HandlerName;

    protected override bool CanHandleInternal(AppActivationArguments args)
    {
        return args.Kind == ExtendedActivationKind.File;
    }

    protected override Task HandleInternalAsync(AppActivationArguments args)
    {
        if (args.Data is IFileActivatedEventArgs fileActivatedEventArgs)
        {
            var fileList = new List<ActivatedFile>();

            foreach (var file in fileActivatedEventArgs.Files)
            {
                fileList.Add(new ActivatedFile
                {
                    Name = file.Name,
                    Path = file.Path,
                    Attributes = (FileAttributes)file.Attributes, // Not really the same, but the flags values match
                    DateCreated = file.DateCreated
                });

                Console.WriteLine($@"Request to [{fileActivatedEventArgs.Verb}] file [{file.Name}][{file.Path}]");
            }

            if (fileActivatedEventArgs.Files.Count > 0)
            {
                _eventAggregator.Publish(new FileActivatedEvent(fileActivatedEventArgs.Verb, fileList));
            }
        }

        return Task.CompletedTask;
    }
}
