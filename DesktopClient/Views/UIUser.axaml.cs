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

        // this must be dependency property
        private int _unreadMessages = 1;

        public int UnreadMessages
        {
            get => _unreadMessages;
            set => _unreadMessages = value; 
        }
    }
}
