using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Labs.Controls;
using VctoonClient.Navigations.Query;

namespace VctoonClient.Navigations.Router;

public class VctoonStackNavigationRouter : StyledElement, IVctoonNavigationRouter
{
    private readonly Menus.NavigationMenuItemProvider _navigationMenuItemProvider;
    private readonly Stack<NavigationRouterPageModel?> _backStack;
    private NavigationRouterPageModel? _currentPage;

    public event EventHandler<NavigatedEventArgs>? Navigated;

    public bool CanGoBack => _backStack?.Count() > 0;

    [Obsolete("Use CurrentPage instead")]
    object? INavigationRouter.CurrentPage { get; }

    public static readonly DirectProperty<VctoonStackNavigationRouter, NavigationRouterPageModel?> CurrentPageProperty =
        AvaloniaProperty.RegisterDirect<VctoonStackNavigationRouter, NavigationRouterPageModel?>(nameof(CurrentPage),
            o => o.CurrentPage,
            (o, v) => o.CurrentPage = v);

    public NavigationRouterPageModel? CurrentPage
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

    public VctoonStackNavigationRouter(Menus.NavigationMenuItemProvider navigationMenuItemProvider)
    {
        _navigationMenuItemProvider = navigationMenuItemProvider;
        _backStack = new Stack<NavigationRouterPageModel?>();
    }

    public Task NavigateToAsync(object? destination, NavigationMode mode = NavigationMode.Normal) =>
        NavigateToAsync(destination, null, mode);

    public async Task BackAsync()
    {
        if (CanGoBack || AllowEmpty)
        {
            CurrentPage = _backStack!.Pop();
            // WeakReferenceMessenger.Default.Send(
            //     new NavigationMessage(CurrentPage as UserControl, CurrentPageParameters));
        }
    }


    public Task NavigateToAsync(string path, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal) =>
        NavigateToAsync((object) path, paras, navigationMode);


    public Task NavigateToAsync(UserControl? view, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal) =>
        NavigateToAsync((object) view, paras, navigationMode);

    public async Task NavigateToAsync(object? pathOrView, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal)
    {
        if (pathOrView == null)
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


        var path = pathOrView as string;

        CurrentPage = new NavigationRouterPageModel((BuildView(pathOrView) as Control)!, path);
    }

    private object? BuildView(object? data)
    {
        if (data == null)
            return new TextBlock {Text = "not find"};

        Control? view = null;
        Dictionary<string, object> paras = null;


        if (data is string path)
        {
            // check has cache
            var stackCache = _backStack.ToList();
            var cache = stackCache.FirstOrDefault(v => v?.Path == path);

            view = cache?.View;

            if (view is null)
            {
                var menuItem = _navigationMenuItemProvider.GetMenuItemByPath(path);

                if (menuItem == null)
                    return new TextBlock {Text = "not find menuItem"};

                if (menuItem.ViewType == null)
                    return new TextBlock {Text = " not find ViewType"};

                view = (UserControl?) App.Services.GetService(menuItem.ViewType!);
            }

            if (view is null)
                return new TextBlock {Text = " not find ViewType"};
        }
        else if (data is UserControl userControl)
        {
            view = userControl;
        }

        // set navigation parameters

        paras = CurrentPageParameters;
        var vm = view.DataContext;

        if (vm != null)
        {
            // QueryPropertyAttribute
            if (!paras.IsNullOrEmpty())
            {
                // TODO: cache
                var queryProperties = vm.GetType().GetCustomAttributes(typeof(QueryPropertyAttribute), true)
                    .Select(v => v as QueryPropertyAttribute).Where(v => v != null);

                foreach (var queryProperty in queryProperties)
                {
                    if (paras.TryGetValue(queryProperty.QueryId, out var value))
                    {
                        if (value != null)
                        {
                            var vmType = vm.GetType();
                            vmType.GetProperty(queryProperty.Name)?.SetValue(vm, value);
                        }
                    }
                }
            }

            // IQueryAttributable
            if (vm is IQueryAttributable vmNavigationQuery)
                vmNavigationQuery.ApplyQueryAttributes(paras);
        }

        // if (data is string)
        //     WeakReferenceMessenger.Default.Send(new NavigationMessage(viewPath, paras));
        // else if (data is UserControl)
        //     WeakReferenceMessenger.Default.Send(new NavigationMessage(view, paras));

        return view;
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