using System.Threading.Tasks;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonClient.ViewModels.Bases;
using VctoonClient.Views;

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

    static LoginViewModel()
    {
        ViewLocator.Register(typeof(LoginViewModel), () =>
        {
            return App.Services.GetService<LoginView>();
        });
    }

    public async Task Login()
    {
        var res = await _loginService.LoginAsync();
        _userStorage.AccessToken = res.AccessToken;
        _userStorage.RefreshToken = res.RefreshToken;
    }
}