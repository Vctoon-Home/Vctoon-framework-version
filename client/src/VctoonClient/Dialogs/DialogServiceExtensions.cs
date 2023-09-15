using System;
using EasyDialog.Avalonia.Dialogs;

namespace VctoonClient.Dialogs;

public static class DialogServiceExtensions
{
    public static IDisposable ShowContentLoading(this DialogService dialogService)
    {
        return dialogService.ShowLoading(DialogConsts.MainViewContentIdentifier);
    }
}