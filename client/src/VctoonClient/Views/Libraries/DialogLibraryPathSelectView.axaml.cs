using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using EasyDialog.Avalonia.Dialogs;
using VctoonClient.ViewModels.Libraries;

namespace VctoonClient.Views.Libraries;

public partial class DialogLibraryPathSelectView : UserControl, ITransientDependency
{
    private readonly DialogLibraryPathSelectViewModel _vm;

    public DialogLibraryPathSelectView()
    {
        InitializeComponent();


        _vm = App.Services.GetRequiredService<DialogLibraryPathSelectViewModel>();
        DataContext = _vm;
        this.UseEasyLoading(_vm.CurrentIdentifier);

        TreeView.ContainerPrepared += TreeView_ContainerPrepared;
    }

    private void TreeView_ContainerPrepared(object sender, ContainerPreparedEventArgs e)
    {
        var treeViewItem = (TreeViewItem) e.Container;
        if (treeViewItem?.DataContext is SystemFolderDtoViewModel treeViewModel)
        {
            treeViewItem.Bind(TreeViewItem.IsExpandedProperty, new Binding(nameof(SystemFolderDtoViewModel.IsExpanded))
            {
                Source = treeViewModel,
                Mode = BindingMode.TwoWay
            });
            treeViewItem.ContainerPrepared += TreeView_ContainerPrepared;
        }
    }
}