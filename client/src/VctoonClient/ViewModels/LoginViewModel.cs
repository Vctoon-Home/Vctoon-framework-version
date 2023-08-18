using System;
using System.IO;
using System.Reactive.Disposables;
using System.Text.Json;
using VctoonClient.Storages;
using VctoonClient.Storages.Apps;

namespace VctoonClient.ViewModels;

public class LoginViewModel : ViewModelBase, ITransientDependency
{
    public LoginViewModel(AppStorage appStorage)
    {
    }
}