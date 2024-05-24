using blazchat.Entities;
using blazchat.Services;
using Microsoft.AspNetCore.SignalR;

namespace blazchat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task JoinChat(Guid chatId, Guid userId)
        {
            if (_chatService.ValidateChat(chatId, userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
            else
            {
                throw new HubException("Unauthorized");
            }
        }

        public async Task SendMessage(Guid chatId, User user, string message, DateTime timestamp)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", user, message, timestamp);
        }
    }
}