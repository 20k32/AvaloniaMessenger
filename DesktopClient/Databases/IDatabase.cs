using System.Collections.Generic;
using DesktopClient.Databases.DTOs;

namespace DesktopClient.Databases;

public interface IDatabase
{
    List<MessagesDbMessageEntry?> GetChatForUser(UsersDbUserEntry friend, MessagesDbUserEntry? user);
    UsersDbUserEntry? GetUserByUserName(string userName);
    string AddUser(UsersDbUserEntry? user);
    string RemoveUser(UsersDbUserEntry? user);
    string AddMessageToUser(UsersDbUserEntry? user, MessagesDbMessageEntry? message);
}