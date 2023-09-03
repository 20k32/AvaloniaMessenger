namespace Shared.Databases.DTOs;

public class MessagesDbUserEntry : RepositoryEntry
{
    public Dictionary<string, List<MessagesDbMessageEntry>> Messages;
    public MessagesDbUserEntry(string userName)
    {
        Messages = new();
        UserName = userName;
        Id = Guid.NewGuid().ToString();
    }

    public override void CopyValuesTo(RepositoryEntry entry)
    {
        if (entry is not MessagesDbUserEntry userEntry)
        {
            return;
        }
        
        userEntry.Messages.Clear();
        foreach (var message in Messages)
        {
            userEntry.Messages.Add(message.Key, message.Value);
        }
    }
}