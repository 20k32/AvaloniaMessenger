using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Models.Auth;
using DesktopClient.Models.Caching;
using DesktopClient.ViewModels;
using DesktopClient.Models.ListBox;
using DesktopClient.Models.Messages;
using DesktopClient.Views;
using DynamicData;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using ReactiveUI;
using Shared.Databases;
using Shared.Databases.DTOs;

namespace DesktopClient.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    private string? _currentChatName = null!;
    private readonly IDatabase _database;
    private readonly UsersDbUserEntry _currentUser;

    public MainWindowViewModel(IDatabase database, UsersDbUserEntry currentUser)
    {
        _database = database;
        _currentUser = currentUser;

        FriendsList.Add(new ListBoxItemCategory("Пользователи в подписках:"));

        foreach (var item in _currentUser.Friends)
        {
            var listItem = new ListBoxItemUser($"{item.FullName} ({item.UserName})", item.UserName);
            UserFriendsCache.Add(listItem, new RelayCommand<string>(DeleteFriend!));
        }

        FriendsList.AddRange(UserFriendsCache.Cached);

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(SearchAsync);
    }

    private void FromMainThread(Action method)
    {
        Dispatcher.UIThread.Invoke(method);
    }

    private void DeleteFriend(string friendUserName)
    {
        /*var friendToDelete = FriendsList.First(u => u.InnerData == friendUserName);
        UserFriendsCache.Remove((friendToDelete as ListBoxItemUser)!);
        FriendsList.Remove(friendToDelete);
        _currentUser.Friends.Remove(_database.GetUserByUserName(friendUserName)!);
        _database.RemoveChatHistoryForUserInChat(friendToDelete.InnerData, _currentUser.UserName);*/
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
            LoadHistoryFor(_selectedFriend.InnerData);
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

            Dispatcher.UIThread.Invoke(() =>
                FriendsList.AddRange(UserFriendsCache.Cached));

            return;
        }

        if (FriendsList.Count != 0)
        {
            FriendsList.Clear();
        }

        Dispatcher.UIThread.Invoke(() =>
            FriendsList.Add(new ListBoxItemCategory("Проводим поиско по базе...")));

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
                if (_currentUser.Friends.OptimizedContains(resultSnapshot[i].UserName))
                {
                    var elem = snapshot.First(snapshotElem => snapshotElem.InnerData == 
                                                              resultSnapshot[i].UserName);
                    friends.AddLast(elem);
                    continue;
                }

                AddGlobalUserToCache(i, $"{resultSnapshot[i].FullName} ({resultSnapshot[i].UserName})",
                    resultSnapshot[i].UserName);
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
    private void AddFriend(string data)
    {
        /*var user = _database.GetUserByUserName(data);
        _currentUser.Friends.Add(user);
        var listItem = new ListBoxItemUser($"{user.FullName}({user.UserName})", user.UserName);
        UserFriendsCache.Add(listItem, new RelayCommand<string>(DeleteFriend!));
        FriendsList.Add(listItem);
        var currentGlobalUser = FriendsList.First(elem => elem.InnerData == user.UserName);
        FriendsList.Remove(currentGlobalUser);*/
    }

    private bool CanExecuteAddFriend(string data)
    {
        /*var user = _database.GetUserByUserName(data);
        return user is not null &&
               !_currentUser.Friends.Contains(user);*/
        return false;
    }

    #endregion
}