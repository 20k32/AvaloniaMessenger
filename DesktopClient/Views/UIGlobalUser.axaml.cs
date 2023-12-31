#region

using Avalonia;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Models;

#endregion

namespace DesktopClient.Views;

public partial class UIGlobalUser : NotifyPropertyChangedUserControl
{
    public UIGlobalUser()
    {
        InitializeComponent();
    }
    public UIGlobalUser(string userName) : this()
    {
        UserNameTextBlock.Text = userName;
    }
    
    #region AddToFriendCommand

    private IRelayCommand<string> _addToFriendCommand;

    public static readonly DirectProperty<UIGlobalUser, IRelayCommand<string>> AddToFriendCommandProperty =
        AvaloniaProperty.RegisterDirect<UIGlobalUser, IRelayCommand<string>>(
            nameof(AddToFriendCommand), 
            o => o.AddToFriendCommand, 
            (o, v) => o.AddToFriendCommand = v);

    public IRelayCommand<string> AddToFriendCommand
    {
        get => _addToFriendCommand;
        set => SetAndRaise(AddToFriendCommandProperty, ref _addToFriendCommand, value);
    }

    #endregion
    
    #region ShortUserName

    private string _shortUserName = string.Empty;
    public string ShortUserName
    {
        get => _shortUserName;
        set
        {
            _shortUserName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region LongUserName

    private string _longUserName = string.Empty;

    public string LongUserName
    {
        get => _longUserName;
        set
        {
            _longUserName = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
}