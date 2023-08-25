using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VctoonClient.Views;

public partial class NavigationView : UserControl, ITransientDependency
{
    public NavigationView()
    {
        InitializeComponent();
    }
}