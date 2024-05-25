namespace HeyGPT.App.Activation;

public interface IActivationHandler
{
    string Name { get; }

    bool CanHandle(object args);

    Task HandleAsync(object args);
}
