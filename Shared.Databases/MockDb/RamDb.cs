using System.Runtime.InteropServices;
using Shared.Databases.DTOs;
using Shared.Databases.MockDb.MessagesDB;
using Shared.Databases.MockDb.UsersDb;

namespace Shared.Databases.MockDb;

// this is mock database class and it's main task is to test other program logic
// and show what methods i must implement in future in mongodb database class.
// i determine to make methods async (in short because i/o ops better to do async)


public class RamDb : IDatabase
{
    private static readonly MessageRamDbAccessor _messages;
    private static readonly UsersRamDbAccessor _users;

    static RamDb()
    {
        _messages = new();
        _users = new();
    }

    public Task<List<MessagesDbMessageEntry>>? GetChatForUserAsync(string friendId, string userId)
    {
        var messageHistory = _messages.GetEntryById(userId);
        
        List<MessagesDbMessageEntry> result = null!;

        if (messageHistory!.Messages.TryGetValue(friendId, out var value))
        {
            result = value.ToList();
        }
        return Task.FromResult(result);
    }

    public Task<UsersDbUserEntry>? GetUserByUserNameAsync(string userName) =>
        Task.FromResult(_users.GetEntryById(userName))!;

    public void AddUserSync(UsersDbUserEntry? user)
    {
        _users.Add(user);
        var userInMessagesDb = new MessagesDbUserEntry(user!.UserName);
        _messages.Add(userInMessagesDb);
    }

    public Task AddUserAsync(UsersDbUserEntry user)
    {
        AddUserSync(user);
        return Task.CompletedTask;
    }
    

    public Task AddMessageToUserAsync(UsersDbUserEntry? user, MessagesDbMessageEntry? message)
    {
        var usersMessageHistory = _messages
            .GetEntryById(user!.UserName)!
            .Messages;

        lock (usersMessageHistory)
        {
            ref var currentChatMesages = 
                ref CollectionsMarshal.GetValueRefOrAddDefault(usersMessageHistory, message!.ChatName, out var exists);

            if (exists)
            {
                currentChatMesages!.Add(message);
            }
            else
            {
                currentChatMesages = new List<MessagesDbMessageEntry>()
                {
                    message
                };
            }
        }
        
        return Task.CompletedTask;
    }

    public Task<IEnumerable<UsersDbUserEntry>>? GetGlobalUsersByUserNameAndFullNameAsync(string options)
    {
        var allUsers = _users.Read();

        Func<UsersDbUserEntry?, bool> selectionPredicate = u =>
        {
            var lowerCaseOptions = options.ToLower();
            
            return u.UserName.ToLower().Contains(lowerCaseOptions)
                   || u.FullName.ToLower().Contains(lowerCaseOptions);
        };

        var result = allUsers
            .Where(selectionPredicate);

        return Task.FromResult(result)!;
    }

    public Task RemoveChatHistoryForUserInChatAsync(string chatName, string userName)
    {
        throw new NotImplementedException();
    }

    public void RemoveUser(UsersDbUserEntry? user)
    {
        _users.Remove(user);
        _messages.Remove(_messages.GetEntryById(user!.UserName));
    }

    public void RemoveChatHistoryForUserInChat(string chatName, string userName)
    {
        var entry = _messages.GetEntryById(userName);
        entry!.Messages.Remove(chatName);
    }
}