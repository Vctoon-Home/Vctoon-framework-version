using System;
using Avalonia.Labs.Controls;
using VctoonClient.Views;

namespace VctoonClient.ViewModels;

public class NavigationViewModel : ViewModelBase, ITransientDependency
{
    static NavigationViewModel()
    {
        ViewLocator.Register(typeof(NavigationViewModel), () =>
        {
            return App.Services.GetService<NavigationView>();
        });
    }

    public INavigationRouter NavigationRouter { get; }

    public NavigationViewModel(INavigationRouter navigationRouter)
    {
        Title = "Navigation";
        NavigationRouter = navigationRouter;

        // NavigateTo(typeof(LoginViewModel));
    }

    public async void NavigateTo(object vmType)
    {
        await NavigationRouter.NavigateToAsync(vmType);
    }
}