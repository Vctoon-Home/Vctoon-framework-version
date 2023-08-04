using Abp.Localization.Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels;

public class ViewModelBase : ObservableObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;
    public LocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<LocalizationManager>()!;
}