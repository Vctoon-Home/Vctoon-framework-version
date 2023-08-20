using System.Threading;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;

namespace VctoonClient.Android.Oidc;

public class AuthenticationBrowser : IBrowser
{
    public Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }
}