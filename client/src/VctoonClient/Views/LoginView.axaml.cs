using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class LoginView : UserControl, ITransientDependency
{
    public LoginView()
    {
        DataContext = App.Services.GetService<LoginViewModel>();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}