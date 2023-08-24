using Avalonia;
using Avalonia.Controls.Converters;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models.Messages
{
    internal sealed class Message
    {
        public MessageBase BaseMessage { get; init; } 
        public HorizontalAlignment Alignment { get; init; }
        public Thickness Margin { get; init; }

        public Message(string messageText, bool isMessageYours)
        {
            BaseMessage = new()
            {
                MessageText = messageText,
                IsMessageYours = Convert.ToInt32(isMessageYours)
            };

            if (isMessageYours)
            {
                Alignment = HorizontalAlignment.Right;
                Margin = new Thickness(0, 5, 2, 0);
            }
            else
            {
                Alignment = HorizontalAlignment.Left;
                Margin = new Thickness(5, 0, 0, 0);
            }

        }
    }
}
