using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Volo.Abp.Http.Client.Authentication;

namespace VctoonClient.Oidc;

public class AvaloniaRemoteServiceHttpClientAuthenticator : IRemoteServiceHttpClientAuthenticator, ITransientDependency
{
    private readonly ILoginService _loginService;

    public AvaloniaRemoteServiceHttpClientAuthenticator(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
    {
        var currentAccessToken = await _loginService.GetAccessTokenAsync();

        if (!currentAccessToken.IsNullOrEmpty())
        {
            context.Request.SetBearerToken(currentAccessToken);
        }
    }
}