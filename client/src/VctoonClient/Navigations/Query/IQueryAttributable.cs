using System.Collections.Generic;

namespace VctoonClient.Navigations.Query;

public interface IQueryAttributable
{
    void ApplyQueryAttributes(Dictionary<string, object>? paras, bool isBack);
}