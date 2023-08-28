using System.Collections.Generic;
using System.Linq;
using DesktopClient.Databases.DTOs;
using DesktopClient.Databases.MockDb.MessagesDB;
using DesktopClient.Databases.MockDb.UsersDb;

namespace DesktopClient.Databases.MockDb;

public class RamDb : IDatabase
{
    private static readonly MessageRamDbAccessor _messages;
    private static readonly UsersRamDbAccessor _users;

    static RamDb()
    {
        _messages = new();
        _users = new();
    }

    public List<MessagesDbMessageEntry?> GetChatForUser(UsersDbUserEntry friend, MessagesDbUserEntry? user)
    {
        return user!.Messages[friend.Id].ToList()!;
    }

    public UsersDbUserEntry? GetUserByUserName(string userName) =>
        _users.GetEntryById(userName);

    public string AddUser(UsersDbUserEntry? user)
    {
        _users.Add(user);
        var userInMessagesDb = new MessagesDbUserEntry(user!.UserName);
        _messages.Add(userInMessagesDb);
        return user.Id;
    }

    public string RemoveUser(UsersDbUserEntry? user)
    {
        _users.Remove(user);
        _messages.Remove(_messages.GetEntryById(user!.UserName));
        return user.Id;
    }
    
    public string AddMessageToUser(UsersDbUserEntry? user, MessagesDbMessageEntry? message)
    {
        _messages
            .GetEntryById(user!.UserName)!
            .Messages
            .Add(message!.Sender, new List<MessagesDbMessageEntry>()
            {
                message
            });

        return message.Id;
    }
}