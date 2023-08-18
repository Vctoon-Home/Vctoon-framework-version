using VctoonClient.Storages.Base;

namespace VctoonClient.Storages.Users;

public class UserStorage : AppStorageBase
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }

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