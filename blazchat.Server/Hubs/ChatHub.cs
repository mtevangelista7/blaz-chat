using blazchat.Application.DTOs;
using blazchat.Application.Interfaces.Services;
using blazchat.Client.Dtos;
using blazchat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace blazchat.Server.Hubs;

public class ChatHub(IChatService chatService, IMessageService messageService) : Hub
{
    public async Task JoinChat(UserJoinChatDto userJoinChatDto)
    {
        if (await chatService.ValidateChat(userJoinChatDto.ChatId, userJoinChatDto.UserId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userJoinChatDto.ChatId.ToString());
        }
        else
        {
            throw new HubException("Unauthorized");
        }
    }

    public async Task SendMessage(SendMessageDto sendMessageDto)
    {
        var msg = new Message
        {
            ChatId = sendMessageDto.ChatId,
            Timestamp = DateTime.UtcNow,
            Text = sendMessageDto.Message,
            UserId = sendMessageDto.UserId
        };

        await messageService.AddMessageAsync(msg);
        await Clients.Group(sendMessageDto.ChatId.ToString()).SendAsync("ReceiveMessage", sendMessageDto.ChatId,
            sendMessageDto.Message, msg.Timestamp);
    }
}