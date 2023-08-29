using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations;

public class NavigationManager
{
    public static IVctoonNavigationRouter MainRouter { get; set; } = new VctoonStackNavigationRouter();
}