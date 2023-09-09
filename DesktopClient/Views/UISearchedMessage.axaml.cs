#region

using Avalonia.Controls;

#endregion

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
