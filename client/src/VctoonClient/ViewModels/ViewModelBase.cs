using CommunityToolkit.Mvvm.ComponentModel;
using VctoonClient.Localizations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels;

public class ViewModelBase : ObservableObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;
    public LocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<LocalizationManager>()!;
}