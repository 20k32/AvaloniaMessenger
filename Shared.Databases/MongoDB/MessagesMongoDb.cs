using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using MongoDB.Driver;
using Shared.Databases.DTOs;
using ZstdSharp.Unsafe;

namespace Shared.Databases.MongoDB;

internal static class MessagesMongoDb
{
    private static readonly IMongoCollection<MessagesDbUserEntry> _messages
        = MongoDbExtensions.GetMessagesCollection();


    public static async Task<MessagesDbUserEntry?> GetUserByUserName(string userName)
    {
        var filter = Builders<MessagesDbUserEntry>.Filter.Eq(e => e.Id, userName);

        var cursor = await _messages.FindAsync(filter);

        var document = await cursor.GetFirstAvailableDocumentAsync();

        return document.FirstOrDefault();
    }

    public static Task AddUser(MessagesDbUserEntry user)
    {
        return _messages.InsertOneAsync(user);
    }

    public static void AddUserSync(MessagesDbUserEntry user)
    {
        _messages.InsertOne(user);
    }
    public static Task RemoveUser(MessagesDbUserEntry user)
    {
        var filter = Builders<MessagesDbUserEntry>.Filter.Eq(e => e.Id, user.Id);
        return _messages.DeleteOneAsync(filter);
    }

    public static Task UpdateUser(MessagesDbUserEntry user)
    {
        var filter = Builders<MessagesDbUserEntry>.Filter.Eq(e => e.Id, user.Id);

        var updateDefinition = new UpdateDefinitionBuilder<MessagesDbUserEntry>()
            .Set(dbElem => dbElem.Messages, user.Messages);

        return _messages.FindOneAndUpdateAsync(filter, updateDefinition);
    }

    public static async Task RemoveUserHistoryForUser(string userName, string chatName)
    {
        var filter = Builders<MessagesDbUserEntry>.Filter.Eq(u => u.UserName, userName);
        var cursor = await _messages.FindAsync(filter);
        var userMessage = await cursor.GetFirstAvailableDocumentAsync();
        var user = userMessage.FirstOrDefault();
        if (user is not null)
        {
            user.Messages.Remove(chatName);
            await UpdateUser(user);
        }
    }

    public static async Task AddMessageToUser(UsersDbUserEntry user, MessagesDbMessageEntry message)
    {
        var filter = Builders<MessagesDbUserEntry>.Filter.Eq(u => u.UserName, user.UserName);
        var cursor = await _messages.FindAsync(filter);

        var document = await cursor.GetFirstAvailableDocumentAsync();

        var userMessages = document.FirstOrDefault();

        if (userMessages == null)
        {
            return;
        }

        if (userMessages.Messages.TryGetValue(message.ChatName, out var messages))
        {
            messages.Add(message);
        }
        else
        {
            var messageList = new List<MessagesDbMessageEntry>()
            {
                message
            };
            userMessages.Messages.Add(user.UserName, messageList);
        }

        await UpdateUser(userMessages);
    }

    public static async Task<List<MessagesDbMessageEntry>?> GetChatForUser(string chatName, string userName)
    {
        var filter = Builders<MessagesDbUserEntry>.Filter.Eq(u => u.UserName, userName);
        var cursor = await _messages.FindAsync(filter);

        var document = await cursor.GetFirstAvailableDocumentAsync();

        var user = document.FirstOrDefault();

        if (user is null)
        {
            return null!;
        }

        List<MessagesDbMessageEntry> result;

        if (user.Messages.TryGetValue(chatName, out var list))
        {
            result = list.ToList();
        }
        else
        {
            result = new();
            user.Messages.Add(chatName, result);

            await UpdateUser(user);
        }

        return result;
    }
}