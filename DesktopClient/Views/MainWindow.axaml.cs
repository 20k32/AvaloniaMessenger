#region

using Avalonia.Controls;
using DesktopClient.ViewModels;
using ReactiveUI;

#endregion

namespace DesktopClient.Views;

public sealed partial class MainWindow : Window
{
    // for designer preview
    public MainWindow()
    { }
    
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        
        viewModel.MainWindow = this;
        DataContext = viewModel;

    }
}
