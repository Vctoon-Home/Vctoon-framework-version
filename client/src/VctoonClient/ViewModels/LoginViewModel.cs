using System.Threading.Tasks;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonClient.Views;

namespace VctoonClient.ViewModels;

public class LoginViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILoginService _loginService;
    private readonly UserStore _userStore;

    public LoginViewModel(ILoginService loginService, UserStore userStore)
    {
        _loginService = loginService;
        _userStore = userStore;
    }

    public async Task Login()
    {
        var res = await _loginService.LoginAsync();
    }
}