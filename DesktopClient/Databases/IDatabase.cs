using System.Collections.Generic;
using DesktopClient.Databases.DTOs;
using DesktopClient.Views;

namespace DesktopClient.Databases;

public interface IDatabase
{
    List<MessagesDbMessageEntry?> GetChatForUser(UsersDbUserEntry friend, UsersDbUserEntry? user);
    List<MessagesDbMessageEntry?>? GetChatForUser(string friendId, string userId);
    UsersDbUserEntry? GetUserByUserName(string userName);
    string AddUser(UsersDbUserEntry? user);
    string RemoveUser(UsersDbUserEntry? user);
    string AddMessageToUser(UsersDbUserEntry? user, MessagesDbMessageEntry? message);
    List<UsersDbUserEntry> GetGlobalUsersByUserNameAndFullName(string options);
}