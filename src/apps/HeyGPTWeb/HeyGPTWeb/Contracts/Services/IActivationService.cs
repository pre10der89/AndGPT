﻿namespace HeyGPTWeb.App.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
