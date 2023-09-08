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

    protected ILocalizationManager L => App.Services.GetRequiredService<ILocalizationManager>();

    protected DialogService DialogService => App.Services.GetRequiredService<DialogService>();

    protected IObjectMapper ObjectMapper => App.Services.GetRequiredService<IObjectMapper>();
}