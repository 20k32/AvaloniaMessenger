using Shared.Databases.DTOs;

namespace Shared.Databases.MockDb.UsersDb;

public class UsersRamDb
{
    public static readonly LinkedList<UsersDbUserEntry> Data;

    static UsersRamDb()
    {
        Data = new();

        var yegorchik = new UsersDbUserEntry("@yegor", "12345", "Yegorchik11111111111111");
        var bob = new UsersDbUserEntry("@bob", "13245", "Bobby");
        var alex = new UsersDbUserEntry("@alex", "23456", "Alex");
        
        yegorchik.Friends.Add(new(bob.FullName, bob.UserName));
        yegorchik.Friends.Add(new(alex.FullName, alex.UserName));
        
        Data.AddLast(yegorchik);
        Data.AddLast(bob);
        Data.AddLast(alex);
    }
}