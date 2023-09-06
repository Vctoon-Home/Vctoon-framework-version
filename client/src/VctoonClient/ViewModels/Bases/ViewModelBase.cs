using System;
using Abp.Localization.Avalonia;
using VctoonClient.Dialogs;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels.Bases;

public abstract class ViewModelBase : ObservableObject
{
    public IAbpLazyServiceProvider? LazyServiceProvider { get; set; }
    
    public ICurrentUser CurrentUser => LazyServiceProvider?.LazyGetRequiredService<ICurrentUser>()!;
    
    protected ILocalizationManager L => LazyServiceProvider?.LazyGetRequiredService<ILocalizationManager>()!;

    protected DialogManager DialogManager => LazyServiceProvider?.LazyGetRequiredService<DialogManager>()!;

    protected Type? ObjectMapperContext { get; set; }

    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
        ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : (IObjectMapper) provider.GetRequiredService(
                typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));
}