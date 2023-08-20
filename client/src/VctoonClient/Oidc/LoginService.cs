using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using IdentityModel.OidcClient;
using VctoonClient.Consts;
using VctoonClient.Storages.Users;

namespace VctoonClient.Oidc;

public class LoginService : ILoginService, ITransientDependency
{
    private readonly OidcClient _oidcClient;
    private readonly UserStorage _userStorage;

    public LoginService(OidcClient oidcClient, UserStorage userStorage)
    {
        _oidcClient = oidcClient;
        _userStorage = userStorage;
    }

    public async Task<LoginResult> LoginAsync()
    {
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

            var result = await _oidcClient.ProcessResponseAsync(response, state);

            if (!result.IsError)
            {
                // focus on the  window
                if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.MainWindow!.Focus();
                }
            }

            return result;
        }
        else
        {
            var result = await _oidcClient.LoginAsync(new LoginRequest());
            return result;
        }
    }

    public async Task<LogoutResult> LogoutAsync()
    {
        var logoutResult = await _oidcClient.LogoutAsync();
        if (!logoutResult.IsError)
        {
            _userStorage.ClearToken();
            // WeakReferenceMessenger.Default.Send(new LogoutMessage());
        }

        return logoutResult;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var token = _userStorage.AccessToken;

        if (!token.IsNullOrEmpty())
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            if (jwtToken.ValidTo <= DateTime.UtcNow)
            {
                var refreshToken = _userStorage.RefreshToken;
                if (!refreshToken.IsNullOrEmpty())
                {
                    var refreshResult = await _oidcClient.RefreshTokenAsync(refreshToken);
                    _userStorage.SetToken(refreshResult.AccessToken, refreshResult.RefreshToken);

                    return refreshResult.AccessToken;
                }

                _userStorage.ClearToken();
                // WeakReferenceMessenger.Default.Send(new LogoutMessage());
            }
        }

        return token;
    }
}