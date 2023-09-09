#region

using Avalonia.Controls;

#endregion

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemCategory : ListBoxItemBase
    {
        public ListBoxItemCategory(string categoryName) : base(string.Empty, isCategory: true)
        {
            Description = new TextBlock()
            {
                Text = categoryName
            };
        }

        public override bool Equals(object? obj) =>
            obj is ListBoxItemCategory category
            && category.Description.Equals(Description);

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
