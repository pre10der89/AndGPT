﻿using Microsoft.Windows.ApplicationModel.Resources;

namespace AndGPT.UI.Core.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader ResourceLoader = new();

    public static string GetLocalized(this string resourceKey) => ResourceLoader.GetString(resourceKey);
}
