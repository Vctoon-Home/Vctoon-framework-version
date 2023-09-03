using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NativeAppStore.Extensions;
using VctoonClient.Navigations.Router;
using VctoonClient.Oidc;
using VctoonClient.Validations;
using VctoonCore;
using VctoonCore.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Collections;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.UI;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;

namespace VctoonClient;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpAutofacModule),
    typeof(AbpHttpClientIdentityModelModule),
    typeof(VctoonCoreHttpApiClientModule),
    typeof(AbpUiModule),
    typeof(AbpLocalizationAvaloniaModule)
)]
public class VctoonClientModule : AbpModule
{
    public async override Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Services.GetConfiguration();
        ConfigureLocalization();

        services.AddLocalizationManager();

        services.AddStores(GetType().Assembly, opt => { opt.EnabledCreatorStoreLoad = true; });

        context.Services.AddTransient<ICurrentPrincipalAccessor, AvaloniaCurrentPrincipalAccessor>();
        context.Services.AddSingleton<IVctoonNavigationRouter, VctoonStackNavigationRouter>();

        Configure<AbpValidationOptions>(options =>
        {
            options.ObjectValidationContributors = new TypeList()
            {
                typeof(VctoonClientDataAnnotationObjectValidationContributor)
            };
        });


        ConfigureOidcClient(context, configuration);


        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<VctoonClientModule>(); });

        // services.
        //
        // services.AddHttpClient("Client")
        //     .AddHttpMessageHandler<VctoonHttpClientHandler>();
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
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddBaseTypes(typeof(AccountResource))
                .AddBaseTypes(typeof(AbpUiResource));

            options.DefaultResourceType = typeof(VctoonCoreResource);

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