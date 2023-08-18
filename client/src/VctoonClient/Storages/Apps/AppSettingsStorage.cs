using System;
using Abp.Localization.Avalonia;
using Newtonsoft.Json;
using VctoonClient.Storages.Base;

namespace VctoonClient.Storages.Apps;

public class AppStorage : AppStorageBase
{
    [JsonIgnore]
    private readonly LocalizationManager _localizationManager;

    [JsonProperty]
    private string CultureName { get; set; }

    public AppStorage(LocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        Load();

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