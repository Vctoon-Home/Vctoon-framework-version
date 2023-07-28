using System;
using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using VctoonCore.Localization;
using Volo.Abp.DependencyInjection;

namespace VctoonClient.Localizations;

public partial class LocalizationManager : ObservableObject, ISingletonDependency
{
    [ObservableProperty] private CultureInfo _currentCulture;

    private readonly IStringLocalizer _localizer;

    public LocalizationManager(IServiceProvider serviceProvider)
    {
        _localizer = serviceProvider.GetRequiredService<IStringLocalizerFactory>()
            .Create(typeof(VctoonCoreResource));
        _currentCulture = CultureInfo.CurrentCulture;
    }

    public LocalizedString this[string resourceKey] => GetValue(resourceKey);

    public LocalizedString GetValue(string resourceKey)
    {
        CultureInfo.CurrentCulture = CurrentCulture;
        CultureInfo.CurrentUICulture = CurrentCulture;

        var val = _localizer[resourceKey];

        return val;
    }
}