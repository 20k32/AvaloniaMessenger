using System.Collections.Generic;
using DesktopClient.Databases.DTOs;

namespace DesktopClient.Databases.MockDb.UsersDb;

public class UsersRamDb
{
    public static readonly LinkedList<UsersDbUserEntry> Data;

    static UsersRamDb()
    {
        Data = new();

        var yegorchik = new UsersDbUserEntry("@yegor", "12345", "Yegorchik11111111111111");
        var bob = new UsersDbUserEntry("@bob", "13245", "Bobby");
        var alex = new UsersDbUserEntry("@alex", "23456", "Alex");
        
        yegorchik.Friends.Add(bob);
        yegorchik.Friends.Add(alex);
        
        bob.Friends.Add(yegorchik);
        
        alex.Friends.Add(yegorchik);
        alex.Friends.Add(bob);
        
        Data.AddLast(yegorchik);
        Data.AddLast(bob);
        Data.AddLast(alex);
    }
}