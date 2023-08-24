using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models.ListBox
{
    internal class ListBoxItemBase
    {
        public string Description { get; init; } = null!;
        public bool IsCategory { get; init; }
        public bool IsUser { get; init; }
        public bool IsMessage { get; init; }

        public ListBoxItemBase(string description, bool isCategory = false, bool isUser = false, bool isMessage = false)
        {
            IsCategory = isCategory;
            IsUser = isUser;
            IsMessage = isMessage;

            Description = description;
        }
    }
}
