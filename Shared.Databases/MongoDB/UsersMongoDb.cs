using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;
using Shared.Databases.DTOs;
using ZstdSharp.Unsafe;

namespace Shared.Databases.MongoDB;

public static class UsersMongoDb
{
    private static readonly IMongoCollection<UsersDbUserEntry> _users = MongoDbExtensions.GetUsersCollection();

    private static UsersDbUserEntry? GetEntryForDocument(IEnumerable<UsersDbUserEntry?>? document)
    {
        if (document == null)
        {
            return null;
        }

        var currentUser = document.FirstOrDefault();

        if (currentUser is null)
        {
            return null;
        }

        return currentUser;
    }

    public static async Task<IList<FriendDbEntry>?> GetFriendsForUser(UsersDbUserEntry user)
    {
        var filter = Builders<UsersDbUserEntry>.Filter.Eq(u => u.Id, user.Id);

        var cursor = await _users.FindAsync(filter);

        var document = await cursor.GetFirstAvaliableDocumentAsync();

        var entry = GetEntryForDocument(document);

        return entry?.Friends ?? null;
    }

    public static Task AddUser(UsersDbUserEntry user)
    {
        return _users.InsertOneAsync(user);
    }

    public static void AddUserSync(UsersDbUserEntry user) =>
        _users.InsertOne(user);

    public static Task RemoveUser(UsersDbUserEntry user)
    {
        var filter = Builders<UsersDbUserEntry>.Filter.Eq(u => u.Id, user.Id);
        return _users.DeleteOneAsync(filter);
    }

    public static async Task<UsersDbUserEntry?> GetUserByUserName(string userName)
    {
        var filter = Builders<UsersDbUserEntry>.Filter.Eq(u => u.UserName, userName);

        var cursor = await _users.FindAsync(filter);

        var document = await cursor.GetFirstAvaliableDocumentAsync();

        return GetEntryForDocument(document)!;
    }

    public static UsersDbUserEntry? GetUserByUserNameSync(string userName)
    {
        var filter = Builders<UsersDbUserEntry>.Filter.Eq(u => u.UserName, userName);

        var cursor = _users.FindSync(filter);

        var document = cursor.GetFirstAvaliableDocumentSync();

        return GetEntryForDocument(document)!;
    }
    
    public static async Task<IEnumerable<UsersDbUserEntry>?> GetUsersByFullName(string fullName)
    {
        var filter = new BsonDocument("FullName", new BsonDocument() { { "$regex", $"{FormatFullName(fullName)}.*" } });

        var cursor = await _users.FindAsync(filter);

        var document = await cursor.GetFirstAvaliableDocumentAsync();

        return document!;
    }

    private static string FormatFullName(string userNameOrFullName) =>
        $"{char.ToUpper(userNameOrFullName[0])}{userNameOrFullName[1..].ToLower()}";

    public static async Task<IEnumerable<UsersDbUserEntry>?> GetUsersByUserName(string userName)
    {
        var filter = new BsonDocument("UserName", new BsonDocument() { { "$regex", $"{userName}.*" } });

        var cursor = await _users.FindAsync(filter);

        var document = await cursor.GetFirstAvaliableDocumentAsync();

        return document!;
    }

    public static void UpdateUserSync(UsersDbUserEntry user)
    {
        var filter = Builders<UsersDbUserEntry>.Filter.Eq(e => e.Id, user.Id);

        var updateDefinition = new UpdateDefinitionBuilder<UsersDbUserEntry>()
            .Set(dbElem => dbElem.Friends, user.Friends);
        
        _users.UpdateOne(filter, updateDefinition);
    }

    public static Task UpdateUserAsync(UsersDbUserEntry user)
    {
        var filter = Builders<UsersDbUserEntry>.Filter.Eq(e => e.Id, user.Id);

        var updateDefinition = new UpdateDefinitionBuilder<UsersDbUserEntry>()
            .Set(dbElem => dbElem.Friends, user.Friends);
        
        return _users.UpdateOneAsync(filter, updateDefinition);
    }
}    