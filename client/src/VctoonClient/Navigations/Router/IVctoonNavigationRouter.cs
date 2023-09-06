using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations.Router;

public interface IVctoonNavigationRouter : INavigationRouter
{
    /// <summary>
    /// CurrentPage can be string or UserControl
    /// </summary>
    new NavigationRouterPageModel? CurrentPage { get; }

    Task NavigateToAsync(string path, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal);

    Task NavigateToAsync(UserControl? view, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal);

    Task NavigateToAsync(object? pathOrView, Dictionary<string, object>? paras = null,
        NavigationMode navigationMode = NavigationMode.Normal);
}