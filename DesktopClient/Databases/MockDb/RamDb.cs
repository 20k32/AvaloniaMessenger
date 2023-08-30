using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

    public List<MessagesDbMessageEntry?> GetChatForUser(UsersDbUserEntry friend, UsersDbUserEntry? user) =>
        GetChatForUser(friend.UserName, user.UserName);

    public List<MessagesDbMessageEntry?>? GetChatForUser(string friendId, string userId)
    {
        var messageHistory = _messages.GetEntryById(userId);
        
        List<MessagesDbMessageEntry> result = null!;

        if (messageHistory!.Messages.TryGetValue(friendId, out var value))
        {
            result = value.ToList();
        }
        return result!;
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
        
        return message.Id;
    }

    public List<UsersDbUserEntry> GetGlobalUsersByUserNameAndFullName(string options)
    {
        var allUsers = _users.Read();

        Func<UsersDbUserEntry?, bool> selectionPredicate = u =>
        {
            var lowerCaseOptions = options.ToLower();
            
            return u.UserName.ToLower().Contains(lowerCaseOptions)
                || u.FullName.ToLower().Contains(lowerCaseOptions);
        };

        return allUsers
            .Where(selectionPredicate)
            .ToList()!;
    }
}