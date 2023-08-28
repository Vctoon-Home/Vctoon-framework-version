using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations;

public class VctoonStackNavigationRouter : StyledElement, IVctoonNavigationRouter
{
    private readonly Stack<object?> _backStack;
    private object? _currentPage;

    public event EventHandler<NavigatedEventArgs>? Navigated;

    public bool CanGoBack => _backStack?.Count() > 0;

    public static readonly DirectProperty<VctoonStackNavigationRouter, object?> CurrentPageProperty =
        AvaloniaProperty.RegisterDirect<VctoonStackNavigationRouter, object?>(nameof(CurrentPage), o => o.CurrentPage,
            (o, v) => o.CurrentPage = v);

    public object? CurrentPage
    {
        get => _currentPage;
        private set
        {
            var oldContent = _currentPage;

            SetAndRaise(CurrentPageProperty, ref _currentPage, value);

            Navigated?.Invoke(this, new NavigatedEventArgs(oldContent, value));
        }
    }
    internal Dictionary<string, object> CurrentPageParameters { get; set; }

    public bool AllowEmpty { get; set; }

    public bool CanGoForward => false;

    public VctoonStackNavigationRouter()
    {
        _backStack = new Stack<object?>();
    }

    public async Task BackAsync()
    {
        if (CanGoBack || AllowEmpty)
        {
            CurrentPage = _backStack?.Pop();
        }
    }

    public Task NavigateToAsync(string path, NavigationMode navigationMode = NavigationMode.Normal)
    {
        return NavigateToAsync(path, null, navigationMode);
    }

    public async Task NavigateToAsync(string? path, Dictionary<string, object>? paras,
        NavigationMode navigationMode = NavigationMode.Normal)
    {
        if (path.IsNullOrEmpty())
        {
            return;
        }

        if (CurrentPage != null)
        {
            switch (navigationMode)
            {
                case NavigationMode.Normal:
                    _backStack.Push(CurrentPage);
                    break;
                case NavigationMode.Clear:
                    _backStack.Clear();
                    break;
            }
        }

        CurrentPageParameters = paras;

        CurrentPage = path;
    }

    // TODO: Add NavigateToAsync for UserControl
    // public async Task NavigateToAsync(UserControl? view, NavigationMode navigationMode = NavigationMode.Normal)
    // {
    //     
    // }

    public Task NavigateToAsync(object? path, NavigationMode navigationMode = NavigationMode.Normal)
    {
        return NavigateToAsync(path?.ToString(), null, navigationMode);
    }

    public async Task ClearAsync()
    {
        _backStack?.Clear();

        if (AllowEmpty)
        {
            CurrentPage = null;
        }
        else
        {
            Navigated?.Invoke(this, new NavigatedEventArgs(CurrentPage, CurrentPage));
        }
    }

    public async Task ForwardAsync()
    {
    }
}