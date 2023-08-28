using System;
using System.Collections.Generic;

namespace DesktopClient.Databases.DTOs;

public class UsersDbUserEntry : RepositoryEntry
{
    public string UserName;
    public string Password;
    public string FullName;
    public List<UsersDbUserEntry> Friends;
    public UsersDbUserEntry(string userName, string password, string fullName) : base() =>
        (UserName, Password, FullName, Friends, Id) = (userName, password, fullName, new(), Guid.NewGuid().ToString());

    public override void CopyValuesTo(RepositoryEntry entry)
    {
        if (entry is not UsersDbUserEntry userEntry)
        {
            return;
        }

        userEntry.UserName = UserName;
        userEntry.FullName = FullName;
        userEntry.Password = Password;
        
        userEntry.Friends.Clear();
        foreach (var item in Friends)
        {
            userEntry.Friends.Add(item);
        }
    }

}