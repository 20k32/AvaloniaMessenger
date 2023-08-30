using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Data;
using DesktopClient.Views;

namespace DesktopClient.Models.ListBox
{
    public sealed class ListBoxItemUser : ListBoxItemBase
    {
        private int _unreadMessages = 0;
        private readonly Subject<int> _source;
        private readonly IDisposable _subscription;
        
        public ListBoxItemUser(string userName, string innerData) : base(innerData, isUser: true)
        {
            var uiUser = new UIUser(userName);

            _source = new Subject<int>();
            _subscription = uiUser.Bind(UIUser.UnreadMessagesProperty, _source, BindingPriority.LocalValue);
            
            Description = uiUser;
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
            
            _source.OnNext(_unreadMessages);
        }

        public void ResetUnreadMessageCount()
        {
            if (_unreadMessages != 0)
            {
                _source.OnNext(_unreadMessages = 0);
            }
        }
        
        public override bool Equals(object? obj) =>
            obj is ListBoxItemUser user
            && user.Description.Equals(Description);

        public override int GetHashCode() =>
            base.GetHashCode();

        public override void Dispose()
        {
            _subscription.Dispose();
        }
    }
}
