using Avalonia.Controls;
using DesktopClient.Models.ListBox;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;

namespace DesktopClient.Views
{
    public sealed partial class AvaliableUsersView : UserControl
    {
        #region Friends

        public static readonly DirectProperty<AvaliableUsersView, ObservableCollection<ListBoxItemBase>>
            FriendsListProperty =
                AvaloniaProperty.RegisterDirect<AvaliableUsersView, ObservableCollection<ListBoxItemBase>>(
                    nameof(FriendsList),
                    getter => getter.FriendsList,
                    (setter, value) => setter.FriendsList = value);

        private ObservableCollection<ListBoxItemBase> _friends = null!;

        public ObservableCollection<ListBoxItemBase> FriendsList
        {
            get => _friends;
            set => SetAndRaise(FriendsListProperty, ref _friends, value);
        }

        #endregion

        #region SelectedFriend

        private ListBoxItemBase _selectedFriend = null!;

        public static readonly DirectProperty<AvaliableUsersView, ListBoxItemBase> SelectedFriendProperty =
            AvaloniaProperty.RegisterDirect<AvaliableUsersView, ListBoxItemBase>(
                nameof(SelectedFriend),
                getter => getter.SelectedFriend,
                (setter, value) => setter.SelectedFriend = value);

        public ListBoxItemBase SelectedFriend
        {
            get => _selectedFriend;
            set => SetAndRaise(SelectedFriendProperty, ref _selectedFriend, value);
        }

        #endregion
        
        #region SearchOptions

        private string _searchOptions;

        public static readonly DirectProperty<AvaliableUsersView, string> SearchOptionsProperty =
            AvaloniaProperty.RegisterDirect<AvaliableUsersView, string>(
                nameof(SearchOptions), 
                o => o.SearchOptions, 
                (o, v) => o.SearchOptions = v);

        public string SearchOptions
        {
            get => _searchOptions;
            set => SetAndRaise(SearchOptionsProperty, ref _searchOptions, value);
        }

        #endregion
        
        public AvaliableUsersView()
        {
            InitializeComponent();
        }
    }
}