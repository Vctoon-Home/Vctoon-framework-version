using System;
using Abp.Localization.Avalonia;
using NativeAppStore.Core;

namespace VctoonClient.Stores.Apps;

[INotifyPropertyChanged]
public partial class AppStore : VctoonStoreBase
{
    private readonly ILocalizationManager _localizationManager;

    [Store]
    private string CultureName { get; set; }

    [ObservableProperty]
    private bool _collapsed;

    public AppStore(ILocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        LoadStore();

        _localizationManager.CurrentCultureChanged += (_, _) => { CultureName = _localizationManager.CurrentCulture.Name; };

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