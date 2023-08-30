using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Data;
using DesktopClient.Views;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemUser : ListBoxItemBase
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
            _source.OnNext(_unreadMessages);
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
