using Avalonia;
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


        #region UnreadMessages

        private int _unreadMessages;

        public static readonly DirectProperty<UIUser, int> UnreadMessagesProperty =
            AvaloniaProperty.RegisterDirect<UIUser, int>(
                nameof(UnreadMessages), o => o.UnreadMessages, (o, v) => o.UnreadMessages = v);

        public int UnreadMessages
        {
            get => _unreadMessages;
            set => SetAndRaise(UnreadMessagesProperty, ref _unreadMessages, value);
        }

        #endregion
    }
}
