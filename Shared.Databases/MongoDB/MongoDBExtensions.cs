using MongoDB.Driver;
using Shared.Databases.DTOs;

namespace Shared.Databases.MongoDB;

public static class MongoDbExtensions
{
    private const string CONNECTION_STRING = "mongodb://localhost:27017";
    private const string DATABASE_NAME = "AvaloniaChat";
    private const string USERS_DOCUMENT_NAME = "UsersDb";
    private const string MESSAGES_DOCUMENT_NAME = "MessagesDb";
    
    private static readonly IMongoDatabase _database = new MongoClient(CONNECTION_STRING)
        .GetDatabase(DATABASE_NAME);

    public static IMongoCollection<T> GetCollectionForDocument<T>(string documentName) =>
        _database.GetCollection<T>(documentName);

    public static IMongoCollection<UsersDbUserEntry> GetUsersCollection() =>
        GetCollectionForDocument<UsersDbUserEntry>(USERS_DOCUMENT_NAME);

    public static IMongoCollection<MessagesDbUserEntry> GetMessagesCollection() =>
        GetCollectionForDocument<MessagesDbUserEntry>(MESSAGES_DOCUMENT_NAME);

    public static IEnumerable<T> GetFirstAvaliableDocumentSync<T>(this IAsyncCursor<T> cursor)
    {
        while (cursor.Current == null)
        {
            cursor.MoveNext();
        }

        return cursor.Current;
    }

    public static async Task<IEnumerable<T?>> GetFirstAvaliableDocumentAsync<T>(this IAsyncCursor<T?> cursor, CancellationToken cts = default)
    {
        while (cursor.Current == null)
        {
            await cursor.MoveNextAsync(cts);
        }

        return cursor.Current;
    }
}