using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models.ListBox
{
    internal class ListBoxItemMessage : ListBoxItemBase
    {
        public ListBoxItemMessage(string description) : base(description, isMessage: true)
        { }
    }
}
