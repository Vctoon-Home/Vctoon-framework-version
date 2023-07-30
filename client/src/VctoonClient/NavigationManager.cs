using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace VctoonClient;

public class NavigationManager : ISingletonDependency
{
    public void ToView<TView>() where TView : UserControl
    {
        ToView(App.Services.GetRequiredService<TView>());
    }

    public void ToView(Type viewType)
    {
        var view = App.Services.GetRequiredService(viewType) as UserControl;

        if (view == null)
            throw new Exception($"View {viewType} not found");

        ToView(view);
    }

    public void ToView(UserControl view)
    {
        var ApplicationLifetime = Application.Current.ApplicationLifetime;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow.Content is UserControl desktopView)
        {
            desktopView.Content = view;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform && singleViewPlatform.MainView is UserControl singleViewPlatformView)
        {
            // singleViewPlatform.MainView = App.Services.GetRequiredService<MainView>();
            singleViewPlatformView.Content = view;
        }
    }

}
