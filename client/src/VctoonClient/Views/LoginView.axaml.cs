using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class LoginView : UserControl, ITransientDependency
{
    public LoginView(LoginViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}