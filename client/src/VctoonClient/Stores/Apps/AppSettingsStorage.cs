using System;
using Abp.Localization.Avalonia;
using NativeAppStore.Core;

namespace VctoonClient.Stores.Apps;

public class AppSettingsStorage : VctoonStoreBase
{
    private readonly LocalizationManager _localizationManager;

    [Store]
    private string CultureName { get; set; }

    public AppSettingsStorage(LocalizationManager localizationManager)
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