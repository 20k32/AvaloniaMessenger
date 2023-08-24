namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemCategory : ListBoxItemBase
    {
        public ListBoxItemCategory(string categoryName) : base(categoryName, isCategory: true)
        { }
    }
}
