using Shared.Databases.DTOs;

namespace Shared.Databases;

public interface IRepositoryAccessor<T> 
    where T : RepositoryEntry
{
    string Add(T? entry);
    string Remove(T? entry);
    string Update(T? entry);
    IList<T> Read();
    T? GetEntryById(string id);
}