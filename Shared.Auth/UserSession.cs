using Shared.Databases.DTOs;

namespace Shared.Auth;

public class UserSession
{
    public UsersDbUserEntry User { get; init; } = null!;
    public string Token { get; init; } = null!;
    public string Role { get; init; } = null!;
    public int ExpiresIn { get; init; }
    public DateTime ExpiryTimeStamp { get; init; }
}