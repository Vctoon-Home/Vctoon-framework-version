using System;
using Abp.Localization.Avalonia;
using Newtonsoft.Json;

namespace VctoonClient.Storages;

public class AppSettingsStorage : StorageBase
{
    public override string? StorageFolder { get; set; }
    public override string StorageFileName { get; set; } = "appsettings.json";

    [JsonIgnore]
    private readonly LocalizationManager _localizationManager;

    [JsonProperty]
    private string CultureName { get; set; }

    public AppSettingsStorage(LocalizationManager localizationManager)
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