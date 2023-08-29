namespace DesktopClient.Databases.DTOs;

public class MessagesDbMessageEntry : RepositoryEntry
{
    public string Data;
    public string ChatName;
    public bool IsYours;
    
    public MessagesDbMessageEntry(bool isYours, string data, string chatName) =>
        (IsYours, Data, ChatName) = (isYours, data, chatName);

    public override void CopyValuesTo(RepositoryEntry entry)
    {
        if (entry is not MessagesDbMessageEntry messageEntry)
        {
            return;
        }

        messageEntry.Data = Data;
        messageEntry.ChatName = ChatName;
        messageEntry.IsYours = IsYours;
    }
}