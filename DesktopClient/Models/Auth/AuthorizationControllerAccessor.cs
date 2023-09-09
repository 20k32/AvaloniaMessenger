using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AspServer.Controllers;
using Shared.Auth;
using Shared.Databases;
using Shared.Databases.DTOs;

namespace DesktopClient.Models.Auth;

public class AuthorizationControllerAccessor : IDisposable
{
    private HttpClient _client;
    private const string AUTHORIZATION_ADDRESS = @"/api/auth/SignIn/Authorize"; 
    
    public AuthorizationControllerAccessor()
    {
        _client = new()
        {
            BaseAddress = new Uri("https://localhost:7298")
        };
    }

    public async Task<UsersDbUserEntry?> RequestAuthFor(UsersDbUserEntry user)
    {
        var authRequest = await _client.PostAsJsonAsync(AUTHORIZATION_ADDRESS, user);
        
        if (authRequest == null)
        {
            return null;
        }
        
        if (authRequest.IsSuccessStatusCode)
        {
            var result =
                await authRequest.Content.ReadFromJsonAsync<UserSession>();

            return result.User;
        }
        
        return null;
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}