using System.Threading.Tasks;
using IdentityModel.OidcClient;

namespace VctoonClient.Oidc;

public interface ILoginService
{
    Task<LoginResult> LoginAsync();

    Task<LogoutResult> LogoutAsync();

    Task<string> GetAccessTokenAsync();
}