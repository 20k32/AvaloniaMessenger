using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemUser : ListBoxItemBase
    {
        public ListBoxItemUser(string userName) : base(userName, isUser: true)
        { }
    }
}
