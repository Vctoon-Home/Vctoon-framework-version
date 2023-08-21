using System.Threading.Tasks;
using VctoonClient.Oidc;
using VctoonClient.Storages.Users;

namespace VctoonClient.ViewModels;

public class LoginViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILoginService _loginService;
    private readonly UserStorage _userStorage;

    public LoginViewModel(ILoginService loginService, UserStorage userStorage)
    {
        _loginService = loginService;
        _userStorage = userStorage;
    }

    public async Task Login()
    {
        var res = await _loginService.LoginAsync();
        _userStorage.AccessToken = res.AccessToken;
        _userStorage.RefreshToken = res.RefreshToken;
    }
}