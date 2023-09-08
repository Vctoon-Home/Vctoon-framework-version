using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyDialog.Avalonia.Dialogs;
using VctoonClient.Messages;
using VctoonCore.Libraries;

namespace VctoonClient.Navigations.Menus;

public class MenuItemViewModel : ViewModelBase
{
    public Guid LibraryId { get; set; }
    public string Header { get; set; }
    public string Path { get; set; }
    public string? Icon { get; set; }

    public bool IsSeparator { get; set; }
    public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();

    public ICommand? ActivateCommand { get; set; }

    public Dictionary<string, object> ClickNavigationParameters { get; set; }

    public Type? ViewType { get; set; }

    public bool IsResource { get; set; }

    public bool IsRootResource { get; set; }


    public async void Delete()
    {
        var dialogService = App.Services.GetRequiredService<DialogService>();

        var res = await dialogService.ShowConfirmAsync(options: opt =>
        {
            opt.Message = @$"Are you sure you want to delete the library ""{Header}""?";
            opt.CloseOnClickAway = true;
        });

        if (!res)
            return;

        using var _ = dialogService.ShowLoading();

        var appService = App.Services.GetRequiredService<ILibraryAppService>();

        try
        {
            await appService.DeleteAsync(LibraryId);
            WeakReferenceMessenger.Default.Send<LibraryDeleteMessage>();

        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }
}