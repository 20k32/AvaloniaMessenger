using Shared.Databases.DTOs;

namespace AspServer.Hubs.ChatHub;

public interface IChatHub
{
    Task SendMessageToUser(MessagesDbMessageEntry message);
}