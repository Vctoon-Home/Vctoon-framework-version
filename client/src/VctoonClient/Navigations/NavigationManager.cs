using Avalonia.Labs.Controls;

namespace VctoonClient.Navigations;

public class NavigationManager
{
    public class Default
    {
        public static INavigationRouter Router { get; set; } = new StackNavigationRouter();
    }
}