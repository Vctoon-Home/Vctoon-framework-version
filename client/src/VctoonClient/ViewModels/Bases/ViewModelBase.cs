using Abp.Localization.Avalonia;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels.Bases;

public abstract class ViewModelBase : ObservableObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;
    public ILocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<ILocalizationManager>()!;
}