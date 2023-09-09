namespace DesktopClient.Models.Messages
{
    public class MessageBase
    {
        public int IsMessageYours { get; init; }
        public string MessageText { get; init; } = null!;
    }
}
