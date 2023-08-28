namespace DesktopClient.Databases.DTOs;

public class MessagesDbMessageEntry : RepositoryEntry
{
    public bool IsYours;
    public string Data = null!;
    public string Sender;
    
    public MessagesDbMessageEntry(bool isYours, string data, string sender) =>
        (IsYours, Data, Sender) = (isYours, data, sender);

    public override void CopyValuesTo(RepositoryEntry entry)
    {
        if (entry is not MessagesDbMessageEntry messageEntry)
        {
            return;
        }

        messageEntry.Data = Data;
        messageEntry.Sender = Sender;
        messageEntry.Sender = Sender;
        messageEntry.IsYours = IsYours;
    }
}