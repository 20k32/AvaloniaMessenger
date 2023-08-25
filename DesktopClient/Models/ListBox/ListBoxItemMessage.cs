using Avalonia.Controls;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemMessage : ListBoxItemBase
    {
        public ListBoxItemMessage(string text) : base(isMessage: true)
        {
            Description = new TextBlock()
            {
                Text = text
            };
        }
    }
}
