using Avalonia.Controls;

namespace VctoonClient.Navigations.Router;

public class NavigationRouterPageModel
{
    public NavigationRouterPageModel(Control view, string? path = null)
    {
        View = view;
        Path = path;
    }

    public Control View { get; set; }
    public string? Path { get; set; }
}