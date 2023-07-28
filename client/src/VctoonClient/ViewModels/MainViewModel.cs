using Volo.Abp.DependencyInjection;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ITransientDependency
{
    public string Greeting => L["Volo.Abp.Identity:010002"].Value.Replace("{MaxUserMembershipCount}", "3");
}