using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Volo.Abp.DependencyInjection;

namespace VctoonClient.Views;

public partial class LoginView : UserControl, ISingletonDependency
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}