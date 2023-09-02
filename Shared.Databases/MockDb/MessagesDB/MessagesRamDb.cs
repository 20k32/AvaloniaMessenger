using Shared.Databases.DTOs;

namespace Shared.Databases.MockDb.MessagesDB;

public class MessagesRamDb
{
    public static readonly LinkedList<MessagesDbUserEntry> Data;

    static MessagesRamDb()
    {
        Data = new();
    }
}