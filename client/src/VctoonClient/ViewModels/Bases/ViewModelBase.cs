using Abp.Localization.Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels.Bases;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private string? _title;
    
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;
    public LocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<LocalizationManager>()!;
}