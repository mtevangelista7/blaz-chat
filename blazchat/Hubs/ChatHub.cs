using blazchat.Entities;
using blazchat.Services;
using Microsoft.AspNetCore.SignalR;

namespace blazchat.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task JoinChat(string chatId, UserDto user)
    {
        var chatGuid = Guid.Parse(chatId);
        var userGuid = user.Id;

        if (await _chatService.ValidateChat(chatGuid, userGuid))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        else
        {
            throw new HubException("Unauthorized");
        }
    }

    public async Task SendMessage(string chatId, UserDto user, string message)
    {
        await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);
    }
}