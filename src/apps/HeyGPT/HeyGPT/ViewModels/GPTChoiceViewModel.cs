namespace HeyGPT.App.ViewModels;
public sealed record GPTChoiceViewModel
{
    public string DisplayName
    {
        get;
        init;
    } = string.Empty;

    public string TransformerName
    {
        get;
        init;
    } = string.Empty;

    public override string ToString() => DisplayName;
}
