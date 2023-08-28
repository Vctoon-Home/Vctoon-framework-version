using System.Collections.Generic;

namespace VctoonClient.Navigations;

public interface INavigationQuery
{
    void OnNavigation(Dictionary<string, object>? paras);
}