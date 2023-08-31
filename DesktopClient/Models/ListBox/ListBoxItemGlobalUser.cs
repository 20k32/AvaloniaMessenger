using System;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Data;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Views;

namespace DesktopClient.Models.ListBox;

public class ListBoxItemGlobalUser : ListBoxItemBase
{
    private readonly Subject<IRelayCommand<string>> _source;
    private readonly IDisposable _subscription;

    public string UserNameDescription
    {
        get => ((UIGlobalUser)Description).LongUserName;
        set => ((UIGlobalUser)Description).LongUserName = value;
    }
    
    public ListBoxItemGlobalUser(string userName) : base(string.Empty, isUser: true)
    {
        var uiUser = new UIGlobalUser(userName);

        _source = new Subject<IRelayCommand<string>>();
        _subscription = uiUser.Bind(UIGlobalUser.AddToFriendCommandProperty, _source, BindingPriority.LocalValue);
        
        Description = uiUser;
    }

    public void SetAddFriendCommand(RelayCommand<string> command) =>
        _source.OnNext(command);

    public void SetInnerData(string data)
    {
        InnerData = data;
        ((UIGlobalUser)Description).ShortUserName = InnerData;
    }
    
    public override void Dispose()
    {
        _subscription.Dispose();
        _source.Dispose();
    }
}