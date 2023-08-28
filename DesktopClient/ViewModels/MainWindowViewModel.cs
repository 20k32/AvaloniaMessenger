using System;
using System.Collections.ObjectModel;
using DesktopClient.Databases;
using DesktopClient.Databases.DTOs;
using DesktopClient.Models.Auth;
using DesktopClient.ViewModels;
using DesktopClient.Models.ListBox;

namespace DesktopClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region FriendsList

    private ObservableCollection<ListBoxItemBase> _friendsList = new();

    public ObservableCollection<ListBoxItemBase> FriendsList
    {
        get => _friendsList;
        set => SetProperty(ref _friendsList, value);
    }

    #endregion
    
    #region SelectedFriend

    private ListBoxItemBase _selectedFriend = null!;

    public ListBoxItemBase SelectedItem
    {
        get => _selectedFriend;
        set
        {
            if (value.IsCategory)
            {
                return;
            }
            
            SetProperty(ref _selectedFriend, value);
        }
    }
    
    #endregion
    
    private readonly IDatabase _database;
    private readonly UsersDbUserEntry _currentUser;
    public MainWindowViewModel(IDatabase database, UsersDbUserEntry currentUser)
    {
        _database = database;
        _currentUser = currentUser;
        
        
        FriendsList.Add(new ListBoxItemCategory("Пользователи в подписках:"));

        foreach (var item in _currentUser.Friends)
        {
            FriendsList.Add(new ListBoxItemUser(item.UserName));
        }
    }
}
