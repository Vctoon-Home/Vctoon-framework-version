using Avalonia.Controls;
using VctoonClient.Dialogs;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    private readonly MainViewModel _vm;


    public MainView()
    {
        InitializeComponent();

        _vm = App.Services.GetService<MainViewModel>()!;
        this.DataContext = _vm;

        App.Services.GetRequiredService<DialogManager>().OnDialogShowLoadingHandler += (s, options, isLoading) =>
        {
            if (s == DialogConsts.MainViewLoadingIdentifier)
            {
                options?.Invoke(this.LoadingContainer);
                this.LoadingContainer.IsLoading = isLoading;
            }
        };
    }
}