using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VctoonClient.Messages;
using VctoonClient.ViewModels;

namespace VctoonClient.Navigations;

public class ViewLocator : IDataTemplate
{
    private readonly MainViewModel _mainViewModel;

    public ViewLocator()
    {
        _mainViewModel = App.Services.GetService<MainViewModel>()!;
    }

    public Control? Build(object? data)
    {
        return (Control?) data;
    }

    public bool Match(object? data) => data is UserControl;
}