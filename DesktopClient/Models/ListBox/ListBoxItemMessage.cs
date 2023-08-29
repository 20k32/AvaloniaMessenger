using Avalonia.Controls;
using DesktopClient.Views;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemMessage : ListBoxItemBase
    {
        public ListBoxItemMessage(string text, string innerData) : base(innerData, isMessage: true)
        {
            Description = new UISearchedMessage(text);
        }
        
        public override bool Equals(object? obj) =>
            obj is ListBoxItemMessage message
            && message.Description.Equals(Description);

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
