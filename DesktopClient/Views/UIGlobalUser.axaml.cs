using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DesktopClient.Views;

public partial class UIGlobalUser : UserControl
{
    public UIGlobalUser()
    {
        InitializeComponent();
    }

    public UIGlobalUser(string userName) : this()
    {
        UserNameTextBlock.Text = userName;
    }
}