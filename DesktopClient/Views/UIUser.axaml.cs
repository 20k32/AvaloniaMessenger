#region

using Avalonia;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Models;

#endregion

namespace DesktopClient.Views
{
    public partial class UIUser : NotifyPropertyChangedUserControl
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

        #region InnerData

        private string _innerData = null!;

        public string InnerData
        {
            get => _innerData;
            set
            {
                _innerData = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DeleteFriendCommand

        private IRelayCommand<string> _deleteFriendCommand;

        public static readonly DirectProperty<UIUser, IRelayCommand<string>> DeleteFriendCommandProperty =
            AvaloniaProperty.RegisterDirect<UIUser, IRelayCommand<string>>(
                nameof(DeleteFriendCommand), 
                o => o.DeleteFriendCommand, 
                (o, v) => o.DeleteFriendCommand = v);

        public IRelayCommand<string> DeleteFriendCommand
        {
            get => _deleteFriendCommand;
            set => SetAndRaise(DeleteFriendCommandProperty, ref _deleteFriendCommand, value);
        }

        #endregion
    }
}
