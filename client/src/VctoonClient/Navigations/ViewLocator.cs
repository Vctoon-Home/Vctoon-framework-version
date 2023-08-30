using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using VctoonClient.Messages;
using VctoonClient.ViewModels;

namespace VctoonClient.Navigations;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        var model = data as NavigationRouterPageModel;

        return model!.View;
    }

    public bool Match(object? data)
    {
        return data is NavigationRouterPageModel;
    }
}