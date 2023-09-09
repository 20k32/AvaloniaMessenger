#region

using System.Buffers;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Models.ListBox;

#endregion

namespace DesktopClient.Models.Caching;

// this class is needed to store a pool of a global clients that current user may search
public static class GlobalUsersCache
{
    private const int ENTRIES_COUNT = 50;
    private static readonly ListBoxItemGlobalUser[] _globalUsers;

    static GlobalUsersCache()
    {
        _globalUsers = ArrayPool<ListBoxItemGlobalUser>.Shared.Rent(ENTRIES_COUNT);

        for (int i = 0; i < ENTRIES_COUNT; i++)
        {
            _globalUsers[i] = new(string.Empty);
        }
    }
    
    
    public static void SetUser(int index, string userName, string innerData, RelayCommand<string> command)
    {
        _globalUsers[index].UserNameDescription = userName;
       _globalUsers[index].SetAddFriendCommand(command);
       _globalUsers[index].SetInnerData(innerData);
    }

    public static IEnumerable<ListBoxItemGlobalUser> GetAllRealUsers()
    {
        for (int i = 0; i < ENTRIES_COUNT; i++)
        {
            if (_globalUsers[i].UserNameDescription != string.Empty)
            {
                yield return _globalUsers[i];
            }
        }
    }

    public static void ResetPool()
    {
        for (int i = 0; i < ENTRIES_COUNT; i++)
        {
            if (_globalUsers[i].UserNameDescription != string.Empty)
            {
                _globalUsers[i].UserNameDescription = string.Empty;
            }
        }
    }
}