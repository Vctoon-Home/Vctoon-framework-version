using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyDialog.Avalonia.Dialogs;
using VctoonClient.Messages;
using VctoonClient.ViewModels.Libraries;
using VctoonClient.Views.Libraries;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;

namespace VctoonClient.Navigations.Menus;

public class MenuItemViewModel : ViewModelBase
{
    public LibraryDto? Library { get; set; }
    public string Header { get; set; }
    public string Path { get; set; }
    public string? Icon { get; set; }

    public bool IsSeparator { get; set; }
    public ObservableCollection<MenuItemViewModel> Children { get; set; } = new ObservableCollection<MenuItemViewModel>();

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
            await appService.DeleteAsync(Library!.Id);
            WeakReferenceMessenger.Default.Send<LibraryDeleteMessage>();

        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }

    public async void Edit()
    {
        var dialogService = App.Services.GetRequiredService<DialogService>();

        using var _ = dialogService.ShowLoading();

        var appService = App.Services.GetRequiredService<ILibraryAppService>();

        try
        {
            var libraryDto = await appService.GetAsync(Library!.Id);
            WeakReferenceMessenger.Default.Send<LibraryDeleteMessage>();

            var library = ObjectMapper.Map<LibraryDto, LibraryCreateUpdateInputViewModel>(libraryDto);

            await App.Router.NavigateToAsync(App.Services.GetRequiredService<LibraryCreateUpdateView>(),
                new Dictionary<string, object>()
                {
                    {"LibraryId", Library!.Id},
                    {"Library", library}
                });
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }
}