using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Messaging;
using IdentityModel.OidcClient;
using VctoonClient.Consts;
using VctoonClient.Messages;
using VctoonClient.Stores.Users;

namespace VctoonClient.Oidc;

public class LoginService : ILoginService, ITransientDependency
{
    private readonly OidcClient _oidcClient;
    private readonly UserStore _userStore;

    public LoginService(OidcClient oidcClient, UserStore userStore)
    {
        _oidcClient = oidcClient;
        _userStore = userStore;
    }

    void WindowFocus()
    {
        // focus on the window
        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow!.Activate();
            desktop.MainWindow!.Focus();
        }
    }


    public async Task<LoginResult> LoginAsync()
    {
        LoginResult? result = null;

        if (Os.IsLinux || Os.IsWindows || Os.IsMacOS)
        {
            var state = await _oidcClient.PrepareLoginAsync();
            var callbackManager = new CallbackManager(state.State);

            Process.Start(new ProcessStartInfo
            {
                FileName = state.StartUrl,
                UseShellExecute = true
            });

            var response = await callbackManager.RunServer();

            result = await _oidcClient.ProcessResponseAsync(response, state);
        }
        else
        {
            result = await _oidcClient.LoginAsync(new LoginRequest());
        }


        if (!result.IsError)
        {
            WindowFocus();
            WeakReferenceMessenger.Default.Send(new LoginMessage());
        }

        return result;
    }

    public async Task<LogoutResult> LogoutAsync()
    {
        var logoutResult = await _oidcClient.LogoutAsync();
        if (!logoutResult.IsError)
        {
            _userStore.ClearToken();
            WeakReferenceMessenger.Default.Send(new LogoutMessage());
        }

        return logoutResult;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var token = _userStore.AccessToken;

        if (!token.IsNullOrEmpty())
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            if (jwtToken.ValidTo <= DateTime.UtcNow)
            {
                var refreshToken = _userStore.RefreshToken;
                if (!refreshToken.IsNullOrEmpty())
                {
                    var refreshResult = await _oidcClient.RefreshTokenAsync(refreshToken);
                    _userStore.SetToken(refreshResult.AccessToken, refreshResult.RefreshToken);

                    return refreshResult.AccessToken;
                }

                _userStore.ClearToken();
                WeakReferenceMessenger.Default.Send(new LogoutMessage());
            }
        }

        return token;
    }
}