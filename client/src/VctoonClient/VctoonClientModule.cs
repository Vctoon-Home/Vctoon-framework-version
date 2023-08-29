using System.Reflection;
using Abp.Localization.Avalonia;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NativeAppStore.Extensions;
using VctoonClient.Oidc;
using VctoonCore;
using VctoonCore.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.UI;

namespace VctoonClient;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpHttpClientIdentityModelModule),
    typeof(VctoonCoreHttpApiClientModule),
    typeof(AbpUiModule),
    typeof(AbpLocalizationAvaloniaModule)
)]
public class VctoonClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Services.GetConfiguration();
        ConfigureLocalization();

        services.AddLocalizationManager(s => s.GetRequiredService<IStringLocalizerFactory>()
            .Create(typeof(VctoonCoreResource)));

        services.AddStores(GetType().Assembly, opt => { opt.EnabledCreatorStoreLoad = true; });
        
        context.Services.AddTransient<ICurrentPrincipalAccessor, AvaloniaCurrentPrincipalAccessor>();

        ConfigureOidcClient(context, configuration);
    }

    private void ConfigureOidcClient(ServiceConfigurationContext context, IConfiguration configuration)
    {
        Configure<OidcClientOptions>(configuration.GetSection("Oidc:Options"));

        context.Services.AddTransient<OidcClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OidcClientOptions>>().Value;

            options.Browser = sp.GetService<IBrowser>();
            var oidcClient = new OidcClient(options);
            return oidcClient;
        });
    }


    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<VctoonCoreResource>()
                .AddBaseTypes(typeof(IdentityResource))
                .AddBaseTypes(typeof(AccountResource))
                .AddBaseTypes(typeof(AbpUiResource));
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية", "ae"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish", "fi"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français", "fr"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский", "ru"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak", "sk"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
        });
    }
}