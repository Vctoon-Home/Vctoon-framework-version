using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace VctoonClient.Navigations;

public class MenuItemViewModel : ViewModelBase
{
    public string MenuHeader { get; set; }
    public string Path { get; set; }
    public string? MenuIconName { get; set; }

    public bool IsSeparator { get; set; }
    public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();

    public ICommand? ActivateCommand { get; set; }

    public Dictionary<string,object> ClickNavigationParameters { get; set; }

    public Type? ViewType { get; set; }
}