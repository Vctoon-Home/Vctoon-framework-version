namespace VctoonClient.Stores.Users;

[INotifyPropertyChanged]
public partial class UserStore : VctoonStoreBase
{
    public TokenInfo TokenInfo { get; protected set; }

    public void ClearToken()
    {
        TokenInfo = null;
    }

    public void SetToken(TokenInfo tokenInfo)
    {
        TokenInfo = tokenInfo;
    }
}