using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations;

public interface IVctoonNavigationRouter : INavigationRouter
{
    /// <summary>
    /// this is the current page 
    /// </summary>
    new object? CurrentPage { get; }

    Task NavigateToAsync(string path, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal);

    Task NavigateToAsync(UserControl? view, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal);

    Task NavigateToAsync(object? pathOrView, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal);
}