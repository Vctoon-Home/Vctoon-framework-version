using System;
using Abp.Localization.Avalonia;
using EasyDialog.Avalonia.Dialogs;
using VctoonClient.Dialogs;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels.Bases;

public abstract class ViewModelBase : ObservableObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }

    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;

    private Lazy<ILocalizationManager> l => new Lazy<ILocalizationManager>(() => App.Services.GetRequiredService<ILocalizationManager>()!);
    protected ILocalizationManager L => l.Value;


    private Lazy<DialogService> dialogService => new Lazy<DialogService>(() => App.Services.GetRequiredService<DialogService>());
    protected DialogService Dialog => dialogService.Value;


    protected IObjectMapper ObjectMapper => App.Services.GetRequiredService<IObjectMapper>();
}