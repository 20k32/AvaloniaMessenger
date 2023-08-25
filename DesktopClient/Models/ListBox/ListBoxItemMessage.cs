using Avalonia.Controls;
using DesktopClient.Views;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemMessage : ListBoxItemBase
    {
        public ListBoxItemMessage(string text) : base(isMessage: true)
        {
            Description = new UISearchedMessage(text);
        }
    }
}
