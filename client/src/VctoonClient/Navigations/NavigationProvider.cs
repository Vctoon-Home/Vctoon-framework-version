using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations;

public class NavigationProvider
{
    public class Default
    {
        public static IVctoonNavigationRouter Router { get; set; } = new VctoonStackNavigationRouter();
    }
}