using System.Collections.Generic;
using DesktopClient.Databases.DTOs;

namespace DesktopClient.Databases.MockDb.MessagesDB;

public class MessagesRamDb
{
    public static readonly LinkedList<MessagesDbUserEntry> Data;

    static MessagesRamDb()
    {
        Data = new();
    }
}