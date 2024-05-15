using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using HeyGPT.App.ViewModels;

#nullable disable

namespace HeyGPT.App.Selectors;

public class ChatMessageTemplateSelector : DataTemplateSelector
{
    public DataTemplate DefaultTemplate
    {
        get; set;
    }

    public DataTemplate LocalTemplate
    {
        get; set;
    }

    public DataTemplate ErrorTemplate
    {
        get; set;
    }

    public DataTemplate PirateTemplate
    {
        get; set;
    }
    public DataTemplate MagicianTemplate
    {
        get; set;
    }
    public DataTemplate TightRopeWalkerTemplate
    {
        get; set;
    }

    public DataTemplate OtherCommunityMemberTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is not ChatMessageReceivedViewModel viewModel)
        {
            return base.SelectTemplateCore(item);
        }

        if (string.Compare(viewModel.CommunityRole, "local", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return LocalTemplate ?? base.SelectTemplateCore(item);
        }

        if (string.Compare(viewModel.CommunityRole, "error", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return ErrorTemplate ?? base.SelectTemplateCore(item);
        }

        if (string.Compare(viewModel.CommunityRole, "default", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return DefaultTemplate ?? base.SelectTemplateCore(item);
        }

        if (string.Compare(viewModel.CommunityRole, "pirate", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return PirateTemplate ?? base.SelectTemplateCore(item);
        }

        if (string.Compare(viewModel.CommunityRole, "magician", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return MagicianTemplate ?? base.SelectTemplateCore(item);
        }

        if (string.Compare(viewModel.CommunityRole, "tightropewalker", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return TightRopeWalkerTemplate ?? base.SelectTemplateCore(item);
        }

        return OtherCommunityMemberTemplate ?? base.SelectTemplateCore(item);
    }
}
