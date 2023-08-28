using System.Collections.Generic;
using System.Linq;
using DesktopClient.Databases.DTOs;

namespace DesktopClient.Databases.MockDb.UsersDb;

internal class UsersRamDbAccessor : IRepositoryAccessor<UsersDbUserEntry>
{
    
    public string Add(UsersDbUserEntry? entry)
    {
        if (entry is null
            || UsersRamDb.Data.Contains(entry!))
        {
            return string.Empty;
        }

        UsersRamDb.Data.AddLast(entry!);

        return entry!.Id;
    }

    public string Remove(UsersDbUserEntry? entry)
    {
        if (entry is null)
        {
            return string.Empty;
        }

        UsersRamDb.Data.Remove(entry!);
        return entry!.Id;
    }

    public string Update(UsersDbUserEntry? entry)
    {
        if (entry is null)
        {
            return string.Empty;
        }

        var searchedEntry = UsersRamDb.Data.FirstOrDefault(x => x.Id == entry!.Id);

        if (searchedEntry is null)
        {
            return string.Empty;
        }
        
        entry!.CopyValuesTo(searchedEntry);

        return searchedEntry.Id;
    }

    public UsersDbUserEntry? GetEntryById(string id) =>
        UsersRamDb.Data.FirstOrDefault(x => x.UserName == id)!;
    
    public IList<UsersDbUserEntry?> Read() =>
        UsersRamDb.Data.ToList()!;
}