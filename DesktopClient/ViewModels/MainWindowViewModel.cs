using System;
using System.Collections.ObjectModel;
using DesktopClient.Databases;
using DesktopClient.Databases.DTOs;
using DesktopClient.Models.Auth;
using DesktopClient.ViewModels;
using DesktopClient.Models.ListBox;
using DesktopClient.Models.Messages;

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
            if (value.IsCategory
                || value.Equals(SelectedItem))
            {
                return;
            }
            
            SetProperty(ref _selectedFriend, value);
            
            var currentUserMessageHistory = _database.GetChatForUser(_selectedFriend.InnerData, _currentUser.UserName);
            
            CurrentChatHistory.Clear();
            
            if (currentUserMessageHistory != null)
            {
                foreach (var item in currentUserMessageHistory)
                {
                    CurrentChatHistory.Add(new ChatMessage(item!.Data, item!.IsYours));
                }
            }
        }
    }
    
    #endregion

    #region CurrentChatHistory

    private ObservableCollection<ChatMessage> _currentChatHistory = new();
    public ObservableCollection<ChatMessage> CurrentChatHistory
    {
        get => _currentChatHistory;
        set => _currentChatHistory = value;
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
            FriendsList.Add(new ListBoxItemUser($"{item.FullName} ({item.UserName})", item.UserName));
        }

        // todo: is yours is unnecessary, sender -> senderId rename!
        _database.AddMessageToUser(_currentUser, new MessagesDbMessageEntry(false, "hi", "@bob"));
        _database.AddMessageToUser(_currentUser, new MessagesDbMessageEntry(true, "hello", "@yegor"));
    }
}
