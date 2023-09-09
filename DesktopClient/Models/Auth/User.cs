#region

using System.Collections.Generic;

#endregion

namespace DesktopClient.Models.Auth;

public class User
{
    public string FullName;
    public string UserName;
    public string Password;
    public List<User> Friends;

    public User(string fullName, string userName, string password) =>
        (FullName, UserName, Password, Friends) = (fullName, userName, password, new());
}