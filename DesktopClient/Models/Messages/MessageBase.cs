using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models.Messages
{
    internal class MessageBase
    {
        public int IsMessageYours { get; init; }
        public string MessageText { get; init; } = null!;
    }
}
