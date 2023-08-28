using VctoonClient.Oidc;

namespace VctoonClient.Layouts.Main;

public partial class MainHeaderViewModel : ViewModelBase, ITransientDependency
{
    [ObservableProperty]
    private bool _isLogin;

    [ObservableProperty]
    public bool _collapsed;

    private readonly ILoginService _loginService;


    public MainHeaderViewModel()
    {
        _loginService = App.Services.GetService<ILoginService>()!;
    }

    public async void Login()
    {
        await _loginService.LoginAsync();
    }
}