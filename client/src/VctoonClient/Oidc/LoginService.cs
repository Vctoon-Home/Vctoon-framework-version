using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
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
        var loginResult = await _oidcClient.LoginAsync(new LoginRequest());

        if (!loginResult.IsError)
        {
            _userStorage.SetToken(loginResult.AccessToken, loginResult.RefreshToken);
            // WeakReferenceMessenger.Default.Send(new LoginMessage());
        }

        return loginResult;
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