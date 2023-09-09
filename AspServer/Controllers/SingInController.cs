using System.Diagnostics.CodeAnalysis;
using AspServer.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Auth;
using Shared.Databases;
using Shared.Databases.DTOs;

namespace AspServer.Controllers
{
    [ApiController, AllowAnonymous]
    [Route("api/auth/[controller]")]
    public class  SignInController: ControllerBase
    {
        private IDatabase _database;
        
        public SignInController(IDatabase Database)
        {
            _database = Database;
        }
        
        [HttpPost(nameof(Authorize)), AllowAnonymous]
        public async Task<ActionResult<UserSession>> Authorize([FromBody] UsersDbUserEntry user)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager(_database);
            var userSession = await jwtAuthenticationManager.GenerateJwtTokenAsync(user);

            if (userSession is null)
            {
                return Unauthorized();
            }

            return userSession;
        }
        
    }
}