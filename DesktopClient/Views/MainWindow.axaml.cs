using Avalonia.Controls;
using DesktopClient.ViewModels;

namespace DesktopClient.Views;

public sealed partial class MainWindow : Window
{
    // for designer preview
    public MainWindow()
    { }

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
