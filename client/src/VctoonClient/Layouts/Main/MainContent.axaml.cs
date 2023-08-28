﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VctoonClient.Navigations;
using VctoonClient.ViewModels;

namespace VctoonClient.Layouts.Main;

public partial class MainContent : UserControl
{
    private MainViewModel _vm;

    public MainContent()
    {
        InitializeComponent();
        _vm = App.Services.GetService<MainViewModel>()!;
        this.DataContext = _vm;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel != null)
        {
            topLevel.BackRequested += TopLevel_BackRequested;
        }
    }

    private async void TopLevel_BackRequested(object? sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as MainViewModel;

        if (viewModel != null)
        {
            if (NavigationProvider.Default.Router.CanGoBack)
            {
                e.Handled = true;

                await NavigationProvider.Default.Router.BackAsync();
            }
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel != null)
        {
            topLevel.BackRequested -= TopLevel_BackRequested;
        }
    }
}