using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using IdentityModel.Client;
using NativeAppStore.Core;
using VctoonClient.Messages;
using VctoonClient.Oidc;

namespace VctoonClient.Stores.Users;

[INotifyPropertyChanged]
public partial class UserStore : VctoonStoreBase
{
    [ObservableProperty]
    private TokenInfo? _tokenInfo;

    public void ClearToken()
    {
        TokenInfo = null;
    }

    public void SetToken(TokenInfo tokenInfo)
    {
        TokenInfo = tokenInfo;
    }
}