using System.Collections.Generic;
using System.Linq;
using DesktopClient.Models.ListBox;

namespace DesktopClient.Models.Caching;

// this cache is used because of decreasing number of bindings and ui objects to create at runtime
public static class UserFriendsCache
{
    private static readonly LinkedList<ListBoxItemUser> _users = new();

    public static IEnumerable<ListBoxItemUser> Cached =>
        _users;

    public static void Add(ListBoxItemUser user)
    {
        _users.AddLast(user);
    }

    public static void Remove(ListBoxItemUser user)
    {
        _users.Remove(user);
    }
}