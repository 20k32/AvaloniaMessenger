#region

using System;
using Avalonia;
using Avalonia.Layout;

#endregion

namespace DesktopClient.Models.Messages
{
    public sealed class ChatMessage
    {
        private static readonly Thickness _yourMessageThickness = new(0, 5, 2, 5);
        private static readonly Thickness _friedMessageThickness = new(2, 5, 0, 5);
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
