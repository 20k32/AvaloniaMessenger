using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Auth;
using Shared.Databases;
using Shared.Databases.DTOs;

namespace AspServer.Auth;

internal sealed class JwtAuthenticationManager
{
    public const string JWT_SECURITY_KEY = "supersecret_test_security_key!342";
    private const int JWT_TOKEN_VALIDITY_MINUTES = 20;
    private readonly IDatabase UserRepository;

    public JwtAuthenticationManager(IDatabase userRepository)
    {
        UserRepository = userRepository;
    }

    public async Task<UserSession?> GenerateJwtTokenAsync(UsersDbUserEntry? user)
    {
        if (user is null)
        {
            return null;
        }

        var userAccount = await UserRepository.GetUserByUserNameAsync(user.UserName);
        
        // user creation
        if (userAccount == null)
        {
            await UserRepository.AddUserAsync(user);
        }
        else if (userAccount.Password != user.Password
                 || userAccount.FullName != user.FullName) // the user is in db but his name or password is invalid
        {
            return null!;
        }
        else
        {
            userAccount.CopyValuesTo(user);
        }
        

        // jwt generation
        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINUTES);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user!.FullName),
            new(ClaimTypes.Role, "user"),
            new(ClaimTypes.NameIdentifier, user.UserName)
        };

        var claimsIdentity = new ClaimsIdentity(claims);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

        var securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };

        var jwtSecurityHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityHandler.WriteToken(securityToken);

        var userSession = new UserSession()
        {
            User = user,
            Role = "user",
            Token = token,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
        };

        return userSession;
    }
}