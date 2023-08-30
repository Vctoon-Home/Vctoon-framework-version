using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Avalonia.Controls;

namespace VctoonClient.Messages;

// public class NavigationMessage
// {
//     public NavigationMessage(string path, Dictionary<string, object> parameters = null)
//     {
//         Path = path;
//         Parameters = parameters ?? Parameters;
//     }
//     public NavigationMessage(UserControl view, Dictionary<string, object> parameters = null)
//     {
//         View = view;
//         Parameters = parameters ?? Parameters;
//     }
//
//     public string? Path { get; set; }
//     public UserControl? View { get; set; }
//
//     public Dictionary<string, object>? Parameters { get; set; } = new Dictionary<string, object>();
// }