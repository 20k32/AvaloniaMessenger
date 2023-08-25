using Avalonia.Controls;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemCategory : ListBoxItemBase
    {
        public ListBoxItemCategory(string categoryName) : base(isCategory: true)
        {
            Description = new TextBlock()
            {
                Text = categoryName
            };
        }
    }
}
