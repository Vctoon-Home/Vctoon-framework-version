﻿using System.Collections.Generic;
using System.Linq;
using Abp.Localization.Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;

namespace VctoonClient.Components;

public class LanguageSelect : UserControl
{
    readonly LocalizationManager _localizationManager;
    private readonly IOptions<AbpLocalizationOptions> _localizationOptions;

    public LanguageSelect()
    {
        _localizationManager = App.Services.GetService<LocalizationManager>();
        _localizationOptions = App.Services.GetService<IOptions<AbpLocalizationOptions>>();
        this.Content = GetMenu();

    }


    public Menu GetMenu()
    {
        var menuFirstItem = new MenuItem()
        {
            Header = _localizationManager.CurrentCulture.DisplayName,
            ItemsSource = GetMenuItemsSource(),
        };

        _localizationManager.PropertyChanged += (_, _) =>
        {
            menuFirstItem.Header = _localizationManager.CurrentCulture.NativeName;
        };

        var menu = new Menu()
        {
            ItemsSource = new List<MenuItem>()
            {
                menuFirstItem
            }
        };

        return menu;
    }

    public List<MenuItem> GetMenuItemsSource() =>
        _localizationOptions.Value.Languages.Select(x => new MenuItem()
        {
            Header = x.DisplayName,
            Command = ReactiveCommand.Create(() => { _localizationManager.ChangeLanguage(x.CultureName); })
        }).ToList();
}