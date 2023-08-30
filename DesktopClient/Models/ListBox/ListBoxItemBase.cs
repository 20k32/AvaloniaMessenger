using System;
using Avalonia.Controls;

namespace DesktopClient.Models.ListBox
{
    public class ListBoxItemBase : IDisposable
    {
        public Control Description { get; init; } = null!;
        // used to located users/messages from db
        public string InnerData { get; init; }
        public bool IsCategory { get; init; }
        public bool IsUser { get; init; }
        public bool IsMessage { get; init; }

        public ListBoxItemBase(string innerData, bool isCategory = false, bool isUser = false, bool isMessage = false)
        {
            IsCategory = isCategory;
            IsUser = isUser;
            IsMessage = isMessage;
            InnerData = innerData;
        }

        public override int GetHashCode() =>
            Description.GetHashCode();

        public virtual void Dispose()
        {
            // todo: release managed resources here
        }
    }
}
