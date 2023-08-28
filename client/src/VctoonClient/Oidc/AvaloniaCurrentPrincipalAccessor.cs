using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;

namespace VctoonClient.Oidc;

public class AvaloniaCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
{
    private readonly ILoginService _loginService;

    public AvaloniaCurrentPrincipalAccessor(ILoginService loginService)
    {
        _loginService = loginService;
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        var tokenString = AsyncHelper.RunSync(() => _loginService.GetAccessTokenAsync());
        if (tokenString.IsNullOrWhiteSpace())
        {
            return new ClaimsPrincipal();
        }

        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
        return new ClaimsPrincipal(new ClaimsIdentity(token.Claims));
    }
}