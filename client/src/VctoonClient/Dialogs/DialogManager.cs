using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Microsoft.Extensions.Options;
using Nito.Disposables;
using Ursa.Controls;
using Volo.Abp;

namespace VctoonClient.Dialogs;

public class DialogManager : ISingletonDependency
{
    private readonly IServiceProvider _serviceProvider;

    public event Action<string, Action<LoadingContainer>, bool> OnDialogShowLoadingHandler;


    public DialogManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TViewModel> ShowAsync<TViewModel>(Control control, string? identifier = null,
        DialogOpenedEventHandler? openedHandler = null, DialogClosingEventHandler? closingHandler = null)
        where TViewModel : class
    {
        var dialog = await DialogHost.Show(control, identifier ?? DialogConsts.MainViewDefaultIdentifier, openedHandler,
            closingHandler);

        var res = ((dynamic) dialog).DataContext;
        return res;
    }

    public Task<TView> ShowAsync<TView>(TView view, string? identifier = null,
        DialogOpenedEventHandler? openedHandler = null, DialogClosingEventHandler? closingHandler = null)
        where TView : Control
    {
        var res = DialogHost.Show(view,
            identifier ?? DialogConsts.MainViewDefaultIdentifier,
            openedHandler,
            closingHandler);

        return res as Task<TView>;
    }

    public async Task<TViewModel> ShowCloseOnClickAwayAsync<TViewModel>(Control control,
        string? identifier = null, DialogOpenedEventHandler? openedHandler = null,
        DialogClosingEventHandler? closingHandler = null)
        where TViewModel : class
    {
        var res = await ShowAsync<TViewModel>(control, identifier,
            (o, args) =>
            {
                var host = args.Source as DialogHost;
                host!.CloseOnClickAway = true;
                openedHandler?.Invoke(o, args);
            }, (o, args) => { });
        return res;
    }

    public async Task<TView> ShowCloseOnClickAwayAsync<TView>(TView view, string? identifier = null,
        DialogOpenedEventHandler? openedHandler = null, DialogClosingEventHandler? closingHandler = null)
        where TView : Control
    {
        var res = await ShowAsync(view, identifier,
            (o, args) =>
            {
                var host = args.Source as DialogHost;
                host!.CloseOnClickAway = true;
                openedHandler?.Invoke(o, args);
            }, (o, args) => { });
        return res;
    }


    // TODO: ShowCustomSimpleEnsureAsync ,has confirm,cancel button options

    // TODO: ShowEnsureAsync


    public IDisposable ShowLoading(string? identifier = null, Action<LoadingContainer> loadingOptions = null)
    {
        // DialogSession? session = null;

        // var loading = new LoadingContainer();
        // loading.IsLoading = true;
        //
        // ShowAsync<object?>(loading,
        //     identifier ?? DialogConsts.MainViewLoadingIdentifier, (o, args) =>
        //     {
        //         var host = args.Source as DialogHost;
        //         host!.CloseOnClickAway = false;
        //         session = args.Session;
        //         // host.Content = loading;
        //
        //         host.Background = Brushes.Transparent;
        //         host.BorderBrush = Brushes.Transparent;
        //         host.BorderThickness = new Thickness();
        //
        //         host.Width = (host.Parent as Control).Width;
        //         host.Height = (host.Parent as Control).Height;
        //         host.DisableOpeningAnimation = false;
        //         loading.Width = host.Width;
        //         loading.Height = host.Height;
        //     }, (o, args) => { });

        // var loading = App.MainView.FindControl<LoadingContainer>(identifier);

        // TODO: need to Create a new LoadingContainer ?
        OnDialogShowLoadingHandler?.Invoke(identifier ?? DialogConsts.MainViewLoadingIdentifier, loadingOptions, true);

        return new DisposeAction(() =>
        {
            OnDialogShowLoadingHandler?.Invoke(identifier ?? DialogConsts.MainViewLoadingIdentifier, loadingOptions,
                false);
        });
    }

    public IDisposable ShowContextLoading()
    {
        return ShowLoading(DialogConsts.MainViewContextLoadingIdentifier);
    }
}