using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Labs.Controls;
using VctoonClient.Messages;
using VctoonClient.ViewModels;

namespace VctoonClient.Navigations;

public class VctoonStackNavigationRouter : StyledElement, IVctoonNavigationRouter
{
    private MainViewModel _mainViewModel;

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

    public Task NavigateToAsync(object? destination, NavigationMode mode = NavigationMode.Normal) =>
        NavigateToAsync(destination, null, mode);

    public async Task BackAsync()
    {
        if (CanGoBack || AllowEmpty)
        {
            CurrentPage = _backStack?.Pop();
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
        CurrentPage = BuildView(pathOrView);
    }

    private Control BuildView(object? data)
    {
        if (data == null)
            return new TextBlock {Text = "not find"};

        UserControl? view = null;
        string viewPath = null;
        Dictionary<string, object> paras = null;

        if (_mainViewModel == null)
            _mainViewModel = App.Services.GetService<MainViewModel>()!;


        if (data is string)
        {
            viewPath = data.ToString()!;
            var menuItems = _mainViewModel.MenuItems;


            var menuItem = menuItems.FirstOrDefault(m => m.Path == viewPath);

            if (menuItem == null)
                return new TextBlock {Text = "not find menuItem"};

            if (menuItem.ViewType == null)
                return new TextBlock {Text = " not find ViewType"};

            view = (UserControl?) App.Services.GetService(menuItem.ViewType!)!;

            if (view == null)
                return new TextBlock {Text = " not find ViewType"};
        }
        else if (data is UserControl userControl)
        {
            view = userControl;
        }

        // set navigation parameters
        if (NavigationManager.MainRouter is VctoonStackNavigationRouter router)
        {
            paras = router.CurrentPageParameters;
            var vm = view.DataContext;

            if (vm == null)
                return view;

            // QueryPropertyAttribute
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
                            vm.GetType().GetProperty(queryProperty.Name)?.SetValue(vm, value);
                        }
                    }
                }
            }

            // IQueryAttributable
            if (vm is IQueryAttributable navigationQuery)
                navigationQuery.ApplyQueryAttributes(paras);
        }

        if (data is string)
            WeakReferenceMessenger.Default.Send(new NavigationMessage(viewPath, paras));
        else if (data is UserControl)
            WeakReferenceMessenger.Default.Send(new NavigationMessage(view, paras));

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