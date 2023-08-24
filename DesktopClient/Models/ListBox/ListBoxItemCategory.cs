using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models.ListBox
{
    internal sealed class ListBoxItemCategory : ListBoxItemBase
    {
        public ListBoxItemCategory(string categoryName) : base(categoryName, isCategory: true)
        { }
    }
}
