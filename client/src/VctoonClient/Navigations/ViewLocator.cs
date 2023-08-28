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
            return new TextBlock {Text = "not find"};

        Control? view = null;

        if (data is string viewPath)
        {
            var menuItems = _mainViewModel.MenuItems;


            var menuItem = menuItems.FirstOrDefault(m => m.Path == viewPath);

            if (menuItem == null)
                return new TextBlock {Text = "not find menuItem"};

            if (menuItem.ViewType == null)
                return new TextBlock {Text = " not find ViewType"};

            view = (Control?) App.Services.GetService(menuItem.ViewType!)!;

            if (view == null)
                return new TextBlock {Text = " not find ViewType"};
        }
        else if (data is Type type)
        {
        }

        // set navigation parameters
        if (NavigationProvider.Default.Router is VctoonStackNavigationRouter router)
        {
            var vm = view.DataContext;

            if (vm == null)
                return view;

            // QueryPropertyAttribute
            {
                // TODO: cache
                var queryProperties = vm.GetType().GetCustomAttributes(typeof(QueryPropertyAttribute), true)
                    .Select(v => v as QueryPropertyAttribute).Where(v => v != null);

                foreach (var queryProperty in queryProperties)
                {
                    if (router.CurrentPageParameters.TryGetValue(queryProperty.QueryId, out var value))
                    {
                        if (value != null)
                        {
                            vm.GetType().GetProperty(queryProperty.Name)?.SetValue(vm, value);
                        }
                    }
                }
            }

            // IQueryAttributable
            if (vm is IQueryAttributable navigationQuery)
                navigationQuery.ApplyQueryAttributes(router.CurrentPageParameters);
        }


        return view;
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