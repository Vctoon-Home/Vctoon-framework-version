using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using VctoonCore.Localization;
using Volo.Abp.Localization;

namespace VctoonClient.Localizations;

public partial class LocalizationManager : ObservableObject, ISingletonDependency
{
    private readonly IOptions<AbpLocalizationOptions> _localizationOptions;
    [ObservableProperty] private CultureInfo _currentCulture;

    private readonly IStringLocalizer _localizer;

    public LocalizationManager(IServiceProvider serviceProvider, IOptions<AbpLocalizationOptions> localizationOptions)
    {
        _localizationOptions = localizationOptions;
        _localizer = serviceProvider.GetRequiredService<IStringLocalizerFactory>()
            .Create(typeof(VctoonCoreResource));
        _currentCulture = CultureInfo.CurrentCulture;
    }

    public List<LanguageInfo> GetSupportLanguages() => _localizationOptions.Value.Languages;

    public LocalizedString this[string resourceKey] => GetValue(resourceKey);


    public void ChangeLanguage(string cultureName)
    {
        CurrentCulture = new CultureInfo(cultureName);
    }

    public LocalizedString GetValue(string resourceKey)
    {
        CultureInfo.CurrentCulture = CurrentCulture;
        CultureInfo.CurrentUICulture = CurrentCulture;

        var val = _localizer[resourceKey];

        return val;
    }
}
