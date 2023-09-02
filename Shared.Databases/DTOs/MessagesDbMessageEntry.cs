namespace Shared.Databases.DTOs;

public class MessagesDbMessageEntry
{
    public string Data;
    public string ChatName;
    public bool IsYours;
    
    public MessagesDbMessageEntry(bool isYours, string data, string chatName) =>
        (IsYours, Data, ChatName) = (isYours, data, chatName);
    
}