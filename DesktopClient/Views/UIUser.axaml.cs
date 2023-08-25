using Avalonia.Controls;

namespace DesktopClient.Views
{
    public partial class UIUser : UserControl
    {
        public UIUser()
        {
            InitializeComponent();
        }

        public UIUser(string userName) : this()
        {
            UserNameTextBlock.Text = userName;
        }
    }
}
