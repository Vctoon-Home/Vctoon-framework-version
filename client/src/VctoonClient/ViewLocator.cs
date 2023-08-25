using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VctoonClient.ViewModels;
using VctoonClient.ViewModels.Bases;
using VctoonClient.Views;

namespace VctoonClient;

public class ViewLocator : IDataTemplate
{
    private static readonly Dictionary<Type, Func<Control>> ViewModelRegistry = new Dictionary<Type, Func<Control>>();

    static ViewLocator()
    {
        ViewModelRegistry.TryAdd(typeof(LoginViewModel), () => App.Services.GetService<LoginView>());
    }

    public static void Register(Type type, Func<Control> factory)
    {
        ViewModelRegistry.TryAdd(type, factory);
    }

    public Control? Build(object? data)
    {
        if (data is null)
            return null;


        if (data is string str)
        {
            var strs = str.ReplaceFirst("//", "").Split("?");

            var viewName = strs.First();

            if (!viewName.EndsWith("viewmodel", StringComparison.OrdinalIgnoreCase))
                viewName += "viewmodel";


            if (strs.Length > 1)
            {
                var query = ParseQueryStringManual(strs.Last());
            }

            return (Control?) ViewModelRegistry.FirstOrDefault(
                r => r.Key.Name.Equals(viewName, StringComparison.OrdinalIgnoreCase)).Value();
        }
        else if (data is Type type)
        {
            if (type != null && ViewModelRegistry.TryGetValue(type, out var factory))
            {
                return factory();
            }
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

    private static Dictionary<string, string> ParseQueryStringManual(string query)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        string[] pairs = query.Split('&');

        foreach (string pair in pairs)
        {
            string[] parts = pair.Split('=', 2);
            if (parts.Length == 2)
            {
                string key = Uri.UnescapeDataString(parts[0]);
                string value = Uri.UnescapeDataString(parts[1]);
                result[key] = value;
            }
        }

        return result;
    }
}