namespace Shared.Databases.DTOs;

public class MessagesDbUserEntry : RepositoryEntry
{
    // key - пользователь в друзьях, value - чат с пользователем
    public readonly Dictionary<string, IList<MessagesDbMessageEntry>> Messages;
    public MessagesDbUserEntry(string userName)
    {
        Messages = new();
        UserName = userName;
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