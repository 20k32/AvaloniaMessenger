using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Databases;
using DesktopClient.Databases.DTOs;
using DesktopClient.Models.Auth;
using DesktopClient.Models.Caching;
using DesktopClient.ViewModels;
using DesktopClient.Models.ListBox;
using DesktopClient.Models.Messages;
using DesktopClient.Views;
using DynamicData;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using ReactiveUI;

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
            UserFriendsCache.Add(new ListBoxItemUser($"{item.FullName} ({item.UserName})", item.UserName));
        }

        FriendsList.AddRange(UserFriendsCache.Cached);
        
        _database.AddMessageToUser(_currentUser,
            new MessagesDbMessageEntry(false,
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                "@yegor"));
        
        _database.AddMessageToUser(_currentUser,
            new MessagesDbMessageEntry(true,
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                "@yegor"));

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(SearchAsync);
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
    
    private ListBoxItemBase _selectedFriend = null!;

    public ListBoxItemBase SelectedItem
    {
        get => _selectedFriend;
        set
        {
            if (value is null 
                || value.IsCategory
                || value.Equals(SelectedItem))
            {
                return;
            }
            
            SetProperty(ref _selectedFriend, value);
            
            if (SelectedItem is ListBoxItemUser user)
            {
                user.ResetUnreadMessageCount();
            }
            
            LoadHistoryFor(_selectedFriend.InnerData);
            _currentChatName = _selectedFriend.InnerData;
            SendMessageCommand.NotifyCanExecuteChanged();
            ChatView.SetFocusToFocusableElement();
        }
    }


    private void LoadHistoryFor(string chatName)
    {
        var currentUserMessageHistory = _database.GetChatForUser(chatName, _currentUser.UserName);
            
        CurrentChatHistory.Clear();

        if (currentUserMessageHistory == null)
        {
            return;
        }
        
        foreach (var item in currentUserMessageHistory)
        {
            CurrentChatHistory.Add(new ChatMessage(item!.Data, item!.IsYours));
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
    private void SendMessage(string text)
    {
        _database.AddMessageToUser(_currentUser, new MessagesDbMessageEntry(true, MessageData, _currentChatName!));
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
    
    #region  SearchText

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
        
        await Task.Delay(1000);

        Dispatcher.UIThread.Invoke(() =>
        {
            FriendsList[0] = new ListBoxItemCategory("Пользователи глобально:");
            
            var result = _database.GetGlobalUsersByUserNameAndFullName(text);
            
            GlobalFriendsCache.ResetPool();
            for (int i = 0; i < result.Count; i++)
            {
                GlobalFriendsCache.SetUser(i, $"{result[i].FullName} ({result[i].UserName})");
            }
            
            FriendsList.AddRange(GlobalFriendsCache.GetAllRealUsers());
        });
       
    }

    #endregion
}
