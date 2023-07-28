using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;

namespace VctoonClient.Components;

public class LanguageSelect : UserControl
{
    public LanguageSelect()
    {
        var localizationOptions = App.Services.GetService<IOptions<AbpLocalizationOptions>>();
        var menuItems = localizationOptions.Value.Languages.Select(x => x.DisplayName).Select(x => new MenuItem()
        {
            Header = x,
        }).ToList();

        var menuItem = new MenuItem()
        {
            Header = "语言",
            ItemsSource = menuItems,
        };

        menuItem.SelectionChanged += (sender, args) =>
        {
            
            
        };


        this.Content = new Menu()
        {
            ItemsSource = new List<MenuItem>()
            {
                menuItem
            }
        };
    }
}