#region

using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Models.ListBox;

#endregion

namespace DesktopClient.Models.Caching;

// this cache is used because of decreasing number of bindings and ui objects to create at runtime
public static class UserFriendsCache
{
    private static readonly LinkedList<ListBoxItemUser> _users = new();

    public static IEnumerable<ListBoxItemUser> Cached =>
        _users;

    public static void Add(ListBoxItemUser user, RelayCommand<string> command)
    {
        user.UpdateFriendsDeletionCommand(command);
        _users.AddLast(user);
    }

    public static void Remove(ListBoxItemUser user)
    {
        _users.Remove(user);
    }

    public static void Clear() =>
        _users.Clear();
}