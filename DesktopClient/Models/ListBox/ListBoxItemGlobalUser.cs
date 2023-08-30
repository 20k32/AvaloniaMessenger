using DesktopClient.Views;

namespace DesktopClient.Models.ListBox;

public class ListBoxItemGlobalUser : ListBoxItemBase
{
    public ListBoxItemGlobalUser(string userName) : base(string.Empty, isUser: true)
    {
        Description = new UIGlobalUser(userName);
    }
}