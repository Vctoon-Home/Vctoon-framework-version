using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using NativeAppStore.Core;
using VctoonClient.Oidc;

namespace VctoonClient.Stores.Users;

[INotifyPropertyChanged]
public partial class UserStore : VctoonStoreBase
{
    // public string RefreshToken { get; set; }
    // public string AccessToken { get; set; }

    [ObservableProperty]
    private string _refreshToken;


    [ObservableProperty]
    private string _accessToken;


    public void ClearToken()
    {
        RefreshToken = null;
        AccessToken = null;
    }

    public void SetToken(string refreshToken, string accessToken)
    {
        RefreshToken = refreshToken;
        AccessToken = accessToken;
    }
}