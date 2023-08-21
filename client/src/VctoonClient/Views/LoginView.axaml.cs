using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>, ITransientDependency
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