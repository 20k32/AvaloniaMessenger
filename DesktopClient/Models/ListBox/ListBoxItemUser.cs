#region

using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Data;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Views;

#endregion

namespace DesktopClient.Models.ListBox
{
    public sealed class ListBoxItemUser : ListBoxItemBase
    {
        private int _unreadMessages = 0;
        private readonly Subject<int> _messageSource;
        private readonly IDisposable _messageSubscription;
        private readonly Subject<IRelayCommand<string>> _commandSource;
        private readonly IDisposable _commandSubscription;
        public string UserNameDescription
        {
            get => ((UIUser)Description).InnerData;
            set => ((UIUser)Description).InnerData = value;
        }
        
        public ListBoxItemUser(string userName, string innerData) : base(innerData, isUser: true)
        {
            var uiUser = new UIUser(userName);

            _messageSource = new Subject<int>();
            _messageSubscription = uiUser.Bind(UIUser.UnreadMessagesProperty, _messageSource, BindingPriority.LocalValue);
            
            _commandSource = new Subject<IRelayCommand<string>>();
            _commandSubscription = uiUser.Bind(UIUser.DeleteFriendCommandProperty, _commandSource,
                BindingPriority.LocalValue);
            
            Description = uiUser;
            UserNameDescription = innerData;
        }

        public void UpdateUnreadMessageCount(int unreadMessagesCount)
        {
            _unreadMessages += unreadMessagesCount;

            if (unreadMessagesCount < 1)
            {
                _unreadMessages = 0;
            }
            else if (_unreadMessages > 99)
            {
                _unreadMessages = 99;
            }
            
            _messageSource.OnNext(_unreadMessages);
        }

        public void ResetUnreadMessageCount()
        {
            if (_unreadMessages != 0)
            {
                _messageSource.OnNext(_unreadMessages = 0);
            }
        }

        public void UpdateFriendsDeletionCommand(IRelayCommand<string> command)
        {
            _commandSource.OnNext(command);
        }
        
        public override bool Equals(object? obj) =>
            obj is ListBoxItemUser user
            && user.Description.Equals(Description);

        public override int GetHashCode() =>
            base.GetHashCode();

        public override void Dispose()
        {
            _messageSubscription.Dispose();
            _messageSource.Dispose();
            _commandSubscription.Dispose();
            _commandSource.Dispose();
        }
    }
}
