using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace VctoonClient.Navigations.Router;

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