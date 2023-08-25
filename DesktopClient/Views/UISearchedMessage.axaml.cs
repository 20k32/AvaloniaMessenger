using Avalonia.Controls;

namespace DesktopClient.Views
{
    public partial class UISearchedMessage : UserControl
    {
        public UISearchedMessage()
        {
            InitializeComponent();
        }

        public UISearchedMessage(string messageText) : this()
        {
            LetterTextBlock.Text = messageText;
        }
    }
}
