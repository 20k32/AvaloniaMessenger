namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemMessage : ListBoxItemBase
    {
        public ListBoxItemMessage(string description) : base(description, isMessage: true)
        { }
    }
}
