using System.Buffers;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using DesktopClient.Models.ListBox;
using DesktopClient.Views;

namespace DesktopClient.Models.Caching;

// this class is needed to store a pool of a global clients that current user may search
public static class GlobalFriendsCache
{
    private const int ENTRIES_COUNT = 50;
    private static readonly ListBoxItemGlobalUser[] _globalUsers;

    static GlobalFriendsCache()
    {
        _globalUsers = ArrayPool<ListBoxItemGlobalUser>.Shared.Rent(ENTRIES_COUNT);

        for (int i = 0; i < ENTRIES_COUNT; i++)
        {
            _globalUsers[i] = new(string.Empty);
        }
    }

    private static TextBlock GetCurrentTextBlock(int index) => 
        ((UIGlobalUser)_globalUsers[index].Description).UserNameTextBlock;
    
    public static void SetUser(int index, string userName)
    {
       GetCurrentTextBlock(index).Text = userName;
    }

    public static IEnumerable<ListBoxItemGlobalUser> GetAllRealUsers()
    {
        for (int i = 0; i < ENTRIES_COUNT; i++)
        {
            var currentTextBlock = GetCurrentTextBlock(i);
            
            if (currentTextBlock.Text != string.Empty)
            {
                yield return _globalUsers[i];
            }
        }
    }

    public static void ResetPool()
    {
        for (int i = 0; i < ENTRIES_COUNT; i++)
        {
            var currentTextBlock = GetCurrentTextBlock(i);
            
            if (currentTextBlock.Text != string.Empty)
            {
                currentTextBlock.Text = string.Empty;
            }
        }
    }
}