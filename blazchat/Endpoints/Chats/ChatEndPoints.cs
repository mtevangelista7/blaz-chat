using blazchat.Client.Dtos;
using blazchat.Entities;
using blazchat.Services.Interfaces;

namespace blazchat.Endpoints.Chats
{
    public static class ChatEndPoints
    {
        public static void MapChatEndpoints(this WebApplication app)
        {
            app.MapPost("/api/chat/create", async (CreateChatDto request, IChatService chatService) =>
            {
                var idChat = await CreateChat(request, chatService);

                if (idChat == Guid.Empty)
                    return Results.NotFound();

                return Results.Ok(idChat);
            });

            app.MapGet("/api/chat/validate/chatId={chatId:guid}/userId={userId:guid}",
                async (Guid chatId, Guid userId, IChatService chatService)
                    =>
                {
                    var isValid = await ValidateChat(chatId, userId, chatService);

                    if (!isValid)
                        return Results.Unauthorized();

                    return Results.Ok();
                });

            app.MapGet("/api/chat/getActiveChats/userId={userId:guid}", async (Guid userId, IChatService chatService) =>
            {
                var activeChats = await chatService.GetActiveChats(userId);
                return Results.Ok(activeChats);
            });
        }

        private static async Task<Guid> CreateChat(CreateChatDto request, IChatService chatService) =>
            await chatService.CreateNewChat(request.User1Id, request.User2Id);

        private static async Task<bool> ValidateChat(Guid chatId, Guid userId, IChatService chatService) =>
            await chatService.ValidateChat(chatId, userId);
    }
}