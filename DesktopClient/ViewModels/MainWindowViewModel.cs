#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Models;
using DesktopClient.Models.Auth;
using DesktopClient.Models.Caching;
using DesktopClient.Models.ListBox;
using DesktopClient.Models.Messages;
using DesktopClient.Views;
using DynamicData;
using ReactiveUI;
using Shared.Databases;
using Shared.Databases.DTOs;

#endregion

namespace DesktopClient.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    public Window MainWindow; 
    
    private string? _currentChatName = null!;
    private readonly IDatabase _database;
    private UsersDbUserEntry _currentUser;
    private readonly AuthorizationControllerAccessor _accessor;
    
    public MainWindowViewModel(IDatabase database, AuthorizationControllerAccessor accessor)
    {
        _database = database;

        //LadFriends();

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(SearchAsync);

        _accessor = accessor;
    }

    private void LoadFriends()
    {
        UserFriendsCache.Clear();
        
        
        FriendsList.Add(new ListBoxItemCategory("Пользователи в подписках:"));

        foreach (var item in _currentUser.Friends)
        {
            var listItem = new ListBoxItemUser($"{item.FullName} ({item.UserName})", item.UserName);
            UserFriendsCache.Add(listItem, new RelayCommand<string>(DeleteFriend!));
        }
        
        FriendsList.AddRange(UserFriendsCache.Cached);
    }
    private void FromMainThread(Action method)
    {
        Dispatcher.UIThread.Invoke(method);
    }

    private async void DeleteFriend(string friendUserName)
    {
        var friendToDelete = FriendsList.First(u => u.InnerData == friendUserName);
        UserFriendsCache.Remove((friendToDelete as ListBoxItemUser)!);
        FriendsList.Remove(friendToDelete);
        var friend = _currentUser.Friends.First(f => f.UserName == friendUserName);
        _currentUser.Friends.Remove(friend);
        await _database.RemoveChatHistoryForUserInChatAsync(friendToDelete.InnerData, _currentUser.UserName);
        await _database.UpdateUserAsync(_currentUser);
    }

    #region FriendsList

    private ObservableCollection<ListBoxItemBase> _friendsList = new();

    public ObservableCollection<ListBoxItemBase> FriendsList
    {
        get => _friendsList;
        set => SetProperty(ref _friendsList, value);
    }

    #endregion

    #region SelectedFriend

    private ListBoxItemBase? _selectedFriend = null!;

    public ListBoxItemBase? SelectedItem
    {
        get => _selectedFriend;
        set
        {
            if (value is null
                || value.IsCategory
                || value is ListBoxItemGlobalUser
                || value.Equals(SelectedItem))
            {
                return;
            }

            SetProperty(ref _selectedFriend, value);
            NotifySendCommandCanExecuteChanged(_selectedFriend.InnerData);
            ChatView.SetFocusToFocusableElement();
            LoadHistoryFor(_selectedFriend.InnerData)
                .DisableAwaitWarning();
        }
    }

    private void NotifySendCommandCanExecuteChanged(string data)
    {
        _currentChatName = data;
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    private async Task LoadHistoryFor(string chatName)
    {
        if (SelectedItem is ListBoxItemUser user)
        {
            user.ResetUnreadMessageCount();
        }

        var currentUserMessageHistory =
            await _database.GetChatForUserAsync(chatName, _currentUser.UserName);

        FromMainThread(CurrentChatHistory.Clear);

        if (currentUserMessageHistory == null)
        {
            return;
        }

        foreach (var item in currentUserMessageHistory)
        {
            FromMainThread(() =>
                CurrentChatHistory.Add(new ChatMessage(item!.Data, item!.IsYours)));
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

    #region MessageData

    private string _messageData = string.Empty;

    public string MessageData
    {
        get => _messageData;
        set
        {
            SetProperty(ref _messageData, value);
            SendMessageCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion

    #region SendMessageCommand

    [RelayCommand(CanExecute = nameof(SendMessageCanExecute))]
    private async Task SendMessage(string text)
    {
        await _database.AddMessageToUserAsync(_currentUser,
            new MessagesDbMessageEntry(true, MessageData, _currentChatName!));

        _currentChatHistory.Add(new ChatMessage(MessageData, true));

        if (SelectedItem is ListBoxItemUser user)
        {
            user.UpdateUnreadMessageCount(1);
        }

        MessageData = string.Empty;
        ChatView.SetFocusToFocusableElement();
    }

    private bool SendMessageCanExecute(string text) =>
        !(string.IsNullOrEmpty(text))
        && !(string.IsNullOrEmpty(_currentChatName));

    #endregion

    #region SearchText

    private string _searchText = null!;

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    #endregion

    #region SearchAsync

    private async void SearchAsync(string? text)
    {
        if (text == null)
        {
            return;
        }

        if (text == string.Empty)
        {
            FriendsList.Clear();

            FromMainThread(() =>
                FriendsList.AddRange(UserFriendsCache.Cached));

            return;
        }

        if (FriendsList.Count != 0)
        {
            FriendsList.Clear();
        }

        FromMainThread(() =>
            FriendsList.Add(new ListBoxItemCategory("Проводим поиск по базе...")));

        var result =
            await _database.GetGlobalUsersByUserNameAndFullNameAsync(text);

        FromMainThread(() =>
        {
            var resultSnapshot = result!.ToArray();

            FriendsList.Clear();

            GlobalUsersCache.ResetPool();
            LinkedList<ListBoxItemBase> friends = new();
            var snapshot = UserFriendsCache.Cached;
            int globalsCount = default;

            for (int i = 0; i < resultSnapshot.Length; i++)
            {
                if (resultSnapshot[i] is null)
                {
                    continue;
                }
                
                if (_currentUser.Friends.OptimizedContains(resultSnapshot[i]!.UserName))
                {
                    var elem = snapshot.First(snapshotElem => snapshotElem.InnerData == 
                                                              resultSnapshot[i].UserName);
                    friends.AddLast(elem);
                    continue;
                }

                AddGlobalUserToCache(i, $"{resultSnapshot[i]!.FullName} ({resultSnapshot[i]!.UserName})",
                    resultSnapshot[i]!.UserName);
                globalsCount++;
            }

            if (globalsCount != 0)
            {
                FriendsList.Add(new ListBoxItemCategory("Пользователи глобально:"));
                FriendsList.AddRange(GlobalUsersCache.GetAllRealUsers());
            }

            if (friends.Count != 0)
            {
                FriendsList.Add(new ListBoxItemCategory("Друзья:"));
                FriendsList.AddRange(friends.AsEnumerable());
            }
        });
    }

    private void AddGlobalUserToCache(int i, string data, string innerData)
    {
        var command = new RelayCommand<string>(AddFriend!, CanExecuteAddFriend!);
        GlobalUsersCache.SetUser(i, data, innerData, command);
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAddFriend))]
    private async void AddFriend(string data)
    {
        var user = await _database.GetUserByUserNameAsync(data);
        _currentUser.Friends.Add(user!);
        var listItem = new ListBoxItemUser($"{user!.FullName}({user.UserName})", user.UserName);
        UserFriendsCache.Add(listItem, new RelayCommand<string>(DeleteFriend!));
        FriendsList.Add(listItem);
        await _database.UpdateUserAsync(_currentUser);
        UpdateUi(user);
    }

    private void UpdateUi(UsersDbUserEntry user)
    {
        var currentGlobalUser = FriendsList.First(elem => elem.InnerData == user.UserName);
        FriendsList.Remove(currentGlobalUser);
    }

    // can't make this async (bullshit)
    private bool CanExecuteAddFriend(string data)
    {
        var user = _database.GetUserByUserNameSync(data);
        return user is not null &&
               !_currentUser.Friends.OptimizedContains(data);
    }

    #endregion
    
    #region Authorization

    [RelayCommand]
    private async Task OpenAuthDialogWindow()
    {
        var window = new LoginWindow(_accessor);
        var user = await window.ShowDialog<UsersDbUserEntry?>(MainWindow);
        _currentUser = user!;
        LoadFriends();
    }
    
    #endregion
}