namespace Shared.Databases.DTOs;

public class UsersDbUserEntry : RepositoryEntry
{
    public string Password;
    public string FullName;
    
    public List<FriendDbEntry> Friends;
    public UsersDbUserEntry(string userName, string password, string fullName) : base() =>
        (UserName, Password, FullName, Friends, Id) = (userName, password, fullName, new(), Guid.NewGuid().ToString());

    public override void CopyValuesTo(RepositoryEntry entry)
    {
        if (entry is not UsersDbUserEntry userEntry)
        {
            return;
        }
        
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