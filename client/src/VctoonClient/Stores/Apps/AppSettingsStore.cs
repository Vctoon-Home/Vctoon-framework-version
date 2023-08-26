using System;
using Abp.Localization.Avalonia;
using NativeAppStore.Core;

namespace VctoonClient.Stores.Apps;

public class AppSettingsStore : VctoonStoreBase
{
    private readonly LocalizationManager _localizationManager;

    [Store]
    private string CultureName { get; set; }

    public AppSettingsStore(LocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        LoadStore();

        _localizationManager.PropertyChanged += (_, _) => { CultureName = _localizationManager.CurrentCulture.Name; };

        if (!CultureName.IsNullOrEmpty())
        {
            _localizationManager.ChangeLanguage(CultureName);
        }
        else
        {
            CultureName = _localizationManager.CurrentCulture.Name;
        }
    }
}