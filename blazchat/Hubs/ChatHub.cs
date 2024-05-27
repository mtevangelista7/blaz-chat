using blazchat.Client.Dtos;
using blazchat.Entities;
using blazchat.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace blazchat.Hubs;

public class ChatHub(IChatService chatService, IMessageService messageService) : Hub
{
    public async Task JoinChat(Guid chatId, UserDto user)
    {
        if (await chatService.ValidateChat(chatId, user.Id))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        else
        {
            throw new HubException("Unauthorized");
        }
    }

    public async Task SendMessage(Guid chatId, UserDto user, string message)
    {
        var msg = new Message
        {
            ChatId = chatId,
            Timestamp = DateTime.UtcNow,
            Text = message,
            UserId = user.Id
        };

        await messageService.AddMessageAsync(msg);

        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", user, message, msg.Timestamp);
    }
}