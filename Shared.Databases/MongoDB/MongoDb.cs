using Shared.Databases.DTOs;

namespace Shared.Databases.MongoDB;

internal class MongoDb : IDatabase
{
    public Task<List<MessagesDbMessageEntry?>?> GetChatForUserAsync(string friendName, string userName) =>
        MessagesMongoDb.GetChatForUser(friendName, userName)!;

    public Task<UsersDbUserEntry?> GetUserByUserNameAsync(string userName) =>
        UsersMongoDb.GetUserByUserName(userName);

    public UsersDbUserEntry? GetUserByUserNameSync(string userName) =>
        UsersMongoDb.GetUserByUserNameSync(userName);

    public void AddUserSync(UsersDbUserEntry? user)
    {
        UsersMongoDb.AddUserSync(user!);
        MessagesMongoDb.AddUserSync(new(user.UserName));
    }

public Task AddUserAsync(UsersDbUserEntry? user)
    {
        var addUserToUserDbTask = UsersMongoDb.AddUser(user!);
        var addUsersMessageHistory = MessagesMongoDb.AddUser(new(user!.UserName));

        return Task.WhenAll(addUserToUserDbTask, addUsersMessageHistory);
    }
    
    public Task AddMessageToUserAsync(UsersDbUserEntry? user, MessagesDbMessageEntry? message) =>
        MessagesMongoDb.AddMessageToUser(user!, message!);

    public async Task<IEnumerable<UsersDbUserEntry?>?> GetGlobalUsersByUserNameAndFullNameAsync(string userNameOrFullName)
    {
        var usersByFullName = await UsersMongoDb.GetUsersByFullName(userNameOrFullName)
                              ?? Enumerable.Empty<UsersDbUserEntry>();

        var usersByUserName = await UsersMongoDb.GetUsersByUserName(userNameOrFullName) 
                              ?? Enumerable.Empty<UsersDbUserEntry>();

        return usersByFullName.Union(usersByUserName);
    }

    public Task RemoveChatHistoryForUserInChatAsync(string chatName, string userName) =>
        MessagesMongoDb.RemoveUserHistoryForUser(userName, chatName);

    public Task UpdateUserAsync(UsersDbUserEntry user) =>
        UsersMongoDb.UpdateUserAsync(user);

    public void UpdateUserSync(UsersDbUserEntry user) =>
        UsersMongoDb.UpdateUserSync(user);
}