using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations;

public interface IVctoonNavigationRouter : INavigationRouter
{
    /// <summary>
    /// this is a Path String
    /// </summary>
    new object? CurrentPage { get; }

    Task NavigateToAsync(string path, NavigationMode navigationMode = NavigationMode.Normal);

    Task NavigateToAsync(string? path, Dictionary<string, object>? paras,
        NavigationMode navigationMode = NavigationMode.Normal);
}