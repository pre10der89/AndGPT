using System.Collections.ObjectModel;
using AndGPT.UI.Core.Contracts.ViewModels;
using AndGPT.WinUI.Core.Contracts.Services;
using AndGPT.WinUI.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AndGPT.WinUI.ViewModels;

public partial class LetsChatViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    public LetsChatViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    [ObservableProperty]
    private SampleOrder? selected;

    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetListDetailsDataAsync();

        foreach (var item in data)
        {
            SampleItems.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected ??= SampleItems.First();
    }
}
