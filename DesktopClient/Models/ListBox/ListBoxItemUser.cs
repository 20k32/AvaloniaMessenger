using DesktopClient.Views;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemUser : ListBoxItemBase
    {
        public ListBoxItemUser(string userName) : base(isUser: true)
        {
            Description = new UIUser();
        }
    }
}
