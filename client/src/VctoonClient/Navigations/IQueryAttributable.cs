using System.Collections.Generic;

namespace VctoonClient.Navigations;

public interface IQueryAttributable
{
    void ApplyQueryAttributes(Dictionary<string, object>? paras);
}