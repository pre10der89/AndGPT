﻿using System.Diagnostics.CodeAnalysis;
using HeyGPTWeb.App.Contracts.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace HeyGPTWeb.App.Services;

public class ChatService : IChatService
{
    private WebView2? _webView;

    public Uri? Source => _webView?.Source;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoBack => false; //_webView != null && _webView.CanGoBack;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoForward => false; //_webView != null && _webView.CanGoForward;

    public event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    public ChatService()
    {
    }

    [MemberNotNull(nameof(_webView))]
    public async Task Initialize(WebView2 webView)
    {
        _webView = webView;
        _webView.NavigationCompleted += OnWebViewNavigationCompleted;

        await _webView.EnsureCoreWebView2Async();
    }

    public void GoBack() 
    {
        //=> _webView?.GoBack();
    }

    public void GoForward()
    {
        //=> _webView?.GoForward();
    }

    public void Reload() => _webView?.Reload();

    public void UnregisterEvents()
    {
        if (_webView != null)
        {
            _webView.NavigationCompleted -= OnWebViewNavigationCompleted;
        }
    }

    private void OnWebViewNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
