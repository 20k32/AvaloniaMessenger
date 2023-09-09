using System.Text.Json.Serialization;

namespace Shared.Databases.DTOs;

public class UsersDbUserEntry : RepositoryEntry
{
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<FriendDbEntry> Friends { get; set; }
    public UsersDbUserEntry(string userName, string password, string fullName) : base() =>
        (UserName, Password, FullName, Friends, Id) = (userName, password, fullName, new(), Guid.NewGuid().ToString());

    public override void CopyValuesTo(RepositoryEntry entry)
    {
        if (entry is not UsersDbUserEntry userEntry)
        {
            return;
        }

        userEntry.Id = Id;
        userEntry.FullName = FullName;
        userEntry.Password = Password;
        
        userEntry.Friends.Clear();
        foreach (var item in Friends)
        {
            userEntry.Friends.Add(item);
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is UsersDbUserEntry user 
               && user.UserName == UserName;
    }

    public override int GetHashCode()
    {
        return $"{UserName}{Password}".GetHashCode();
    }
}