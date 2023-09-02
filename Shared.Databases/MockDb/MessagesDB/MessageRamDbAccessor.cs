using Shared.Databases.DTOs;

namespace Shared.Databases.MockDb.MessagesDB;

internal class MessageRamDbAccessor : IRepositoryAccessor<MessagesDbUserEntry>
{
    public string Add(MessagesDbUserEntry? entry)
    {
        if (entry is null)
        {
            return string.Empty;
        }
        
        MessagesRamDb.Data.AddLast(entry!);
        return entry!.Id;
    }

    public string Remove(MessagesDbUserEntry? entry)
    {
        if (entry is null)
        {
            return string.Empty;
        }
        MessagesRamDb.Data.Remove(entry!);
        return entry!.Id;
    }

    public string Update(MessagesDbUserEntry? entry)
    {
        if (entry is null)
        {
            return string.Empty;
        }
        
        var searchedEntry = MessagesRamDb.Data.FirstOrDefault(x => x.Id == entry!.Id);
        
        if (searchedEntry is null)
        {
            return string.Empty;
        }
        
        entry!.CopyValuesTo(searchedEntry);
        
        return entry.Id;
    }

    public IList<MessagesDbUserEntry> Read()
    {
        return MessagesRamDb.Data.ToList()!;
    }

    public MessagesDbUserEntry? GetEntryById(string id) =>
        MessagesRamDb.Data.FirstOrDefault(x => x.Id == id);
}