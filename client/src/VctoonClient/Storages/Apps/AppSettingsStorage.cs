using System;
using Abp.Localization.Avalonia;
using Newtonsoft.Json;
using VctoonClient.Storages.Base;

namespace VctoonClient.Storages.Apps;

public class AppStorage : AppStorageBase
{
    private readonly LocalizationManager _localizationManager;

    [Storage]
    private string CultureName { get; set; }

    public AppStorage(LocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        LoadStorage();

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