using blazchat.Client.Dtos;
using blazchat.Client.RefitInterfaceApi;
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

            app.MapPost("/api/chat/validate",
                async (ValidateChatDto validateChat, IChatService chatService)
                    =>
                {
                    var isValid = await chatService.ValidateChat(validateChat.ChatId, validateChat.UserId);

                    if (!isValid)
                        return Results.Ok(false);

                    return Results.Ok(true);
                });

            app.MapGet("/api/chat/getActiveChats/{userId:guid}", async (Guid userId, IChatService chatService) =>
            {
                try
                {
                    var activeChats = await chatService.GetActiveChats(userId);
                    return Results.Ok(activeChats);
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
            });
        }

        private static async Task<Guid> CreateChat(CreateChatDto request, IChatService chatService) =>
            await chatService.CreateNewChat(request.User1Id, request.User2Id);
    }
}