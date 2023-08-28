using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VctoonClient.ViewModels;

namespace VctoonClient.Navigations;

public class ViewLocator : IDataTemplate
{
    private readonly MainViewModel _mainViewModel;

    public ViewLocator()
    {
        _mainViewModel = App.Services.GetService<MainViewModel>()!;
    }

    public Control? Build(object? data)
    {
        if (data == null)
            return null;


        if (data is string viewPath)
        {
            var menuItems = _mainViewModel.MenuItems;


            var menuItem = menuItems.FirstOrDefault(m => m.Path == viewPath);

            if (menuItem == null)
                return new TextBlock {Text = "not find menuItem"};

            if (menuItem.ViewType == null)
                return new TextBlock {Text = " not find ViewType"};

            var view = (Control?) App.Services.GetService(menuItem.ViewType!)!;

            if (view == null)
                return new TextBlock {Text = " not find ViewType"};

            if (NavigationProvider.Default.Router is VctoonStackNavigationRouter router)
            {
                var vm = view.DataContext;

                if (vm == null)
                    return view;

                // 判断vm是否继承INavigationQuery
                if (vm is INavigationQuery navigationQuery)
                {
                    navigationQuery.OnNavigation(router.CurrentPageParameters);
                }
            }

            return view;
        }
        else if (data is Type type)
        {

        }


        return new TextBlock {Text = "not find"};
    }

    public bool Match(object? data)
    {
        if (data is string str && !str.IsNullOrEmpty() && str.StartsWith("//"))
        {
            return true;
        }
        else if (data is Type)
        {
            return true;
        }

        return false;
    }

}