using Avalonia.Controls;
using DesktopClient.ViewModels;

namespace DesktopClient.Views;

internal sealed partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
