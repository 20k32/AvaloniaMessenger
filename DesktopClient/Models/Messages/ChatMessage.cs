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
    public sealed class ChatMessage
    {
        private static readonly Thickness _yourMessageThickness = new(0, 5, 2, 0);
        private static readonly Thickness _friedMessageThickness = new(5, 2, 0, 0);
        public MessageBase BaseMessage { get; } 
        public HorizontalAlignment Alignment { get; init; }
        public Thickness Margin { get; init; }

        public ChatMessage(string messageText, bool isMessageYours)
        {
            BaseMessage = new()
            {
                MessageText = messageText,
                IsMessageYours = Convert.ToInt32(isMessageYours)
            };

            if (isMessageYours)
            {
                Alignment = HorizontalAlignment.Right;
                Margin = _yourMessageThickness;
            }
            else
            {
                Alignment = HorizontalAlignment.Left;
                Margin = _friedMessageThickness;
            }

        }
    }
}
