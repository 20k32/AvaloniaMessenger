using Avalonia.Controls;

namespace DesktopClient.Models.ListBox
{
    public class ListBoxItemBase
    {
        public Control Description { get; init; } = null!;
        public bool IsCategory { get; init; }
        public bool IsUser { get; init; }
        public bool IsMessage { get; init; }

        public ListBoxItemBase(bool isCategory = false, bool isUser = false, bool isMessage = false)
        {
            IsCategory = isCategory;
            IsUser = isUser;
            IsMessage = isMessage;
        }
    }
}
