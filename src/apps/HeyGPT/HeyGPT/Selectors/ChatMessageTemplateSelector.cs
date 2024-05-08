using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using HeyGPT.App.ViewModels;
using HeyGPT.Core.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

#nullable disable

namespace HeyGPT.App.Selectors;

public class ChatMessageTemplateSelector : DataTemplateSelector
{
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
