namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemUser : ListBoxItemBase
    {
        public ListBoxItemUser(string userName) : base(userName, isUser: true)
        { }
    }
}
