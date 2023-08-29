using DesktopClient.Views;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemUser : ListBoxItemBase
    {
        public ListBoxItemUser(string userName, string innerData) : base(innerData, isUser: true)
        {
            Description = new UIUser(userName);
        }
        
        public override bool Equals(object? obj) =>
            obj is ListBoxItemUser user
            && user.Description.Equals(Description);

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
