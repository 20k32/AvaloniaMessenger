using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Databases.DTOs;

namespace AspServer.Hubs.ChatHub;

public class ChatHub : Hub<IChatHub>
{
    [Authorize]
    public async Task SendToUser(MessagesDbMessageEntry message)
    {
        await Clients.Caller.SendMessageToUser(message);
        await Clients.User(message.ChatName).SendMessageToUser(message);
    }
}