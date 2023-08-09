using Abp.Localization.Avalonia;
using ReactiveUI;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;
    public LocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<LocalizationManager>()!;
}