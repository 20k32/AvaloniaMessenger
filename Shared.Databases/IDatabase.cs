using Shared.Databases.DTOs;

namespace Shared.Databases;

public interface IDatabase
{
    Task<List<MessagesDbMessageEntry?>?> GetChatForUserAsync(string friendId, string userId);
    Task<UsersDbUserEntry?> GetUserByUserNameAsync(string userName);
    Task AddUserAsync(UsersDbUserEntry? user);
    void AddUserSync(UsersDbUserEntry? user);
    Task AddMessageToUserAsync(UsersDbUserEntry? user, MessagesDbMessageEntry? message);
    Task<IEnumerable<UsersDbUserEntry?>?> GetGlobalUsersByUserNameAndFullNameAsync(string options);
    Task RemoveChatHistoryForUserInChatAsync(string chatName, string userName);
}