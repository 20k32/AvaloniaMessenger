namespace Shared.Databases.DTOs;

public class FriendDbEntry
{
    public string FullName;
    public string UserName;

    public FriendDbEntry(string fullName, string userName) =>
        (FullName, UserName) = (fullName, userName);

    public static implicit operator FriendDbEntry(UsersDbUserEntry user) =>
        new(user.FullName, user.UserName);

    public override bool Equals(object? obj) =>
        obj is FriendDbEntry entry
        && entry.UserName == UserName;
    
    public override int GetHashCode()
    {
        return UserName!.GetHashCode();
    }
}

public static class FriendDbEntryCollectionExtensions
{
    private static FriendDbEntry dummyEntry = new(string.Empty, string.Empty);

    public static bool OptimizedContains(this IEnumerable<FriendDbEntry> collection, string userName)
    {
        dummyEntry.UserName = userName;
        
        foreach (var item in collection)
        {
            if (item.Equals(dummyEntry))
            {
                return true;
            }
        }

        return false;
    }
}