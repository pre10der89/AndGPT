﻿using HeyGPT.App.Contracts.Services;
using HeyGPT.App.ViewModels;
using Microsoft.UI.Xaml;

namespace HeyGPT.App.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(LetsChatViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
