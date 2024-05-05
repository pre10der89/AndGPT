namespace AndGPT.WinUI.ViewModels;
public sealed record TransformerChoiceViewModel
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
