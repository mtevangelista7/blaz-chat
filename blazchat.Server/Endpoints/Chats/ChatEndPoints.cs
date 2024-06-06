using blazchat.Application.DTOs;
using blazchat.Application.Interfaces.Services;

namespace blazchat.Server.Endpoints.Chats
{
    public static class ChatEndPoints
    {
        public static void MapChatEndpoints(this WebApplication app)
        {
            app.MapPost("/api/chat/create", async (CreateChatDto request, IChatService chatService) =>
            {
                try
                {
                    var idChat = await chatService.CreateNewChat(request.User1Id, request.User2Id);
                    return idChat == Guid.Empty ? Results.NotFound() : Results.Ok(idChat);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            });

            app.MapPost("/api/chat/validate",
                async (ValidateChatDto validateChat, IChatService chatService)
                    =>
                {
                    try
                    {
                        var isValid = await chatService.ValidateChat(validateChat.ChatId, validateChat.UserId);
                        return Results.Ok(isValid);
                    }
                    catch (Exception e)
                    {
                        return Results.BadRequest(e.Message);
                    }
                });

            app.MapGet("/api/chat/getActiveChats/{userId:guid}", async (Guid userId, IChatService chatService) =>
            {
                try
                {
                    var activeChats = await chatService.GetActiveChats(userId);
                    return Results.Ok(activeChats);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            });
        }
    }
}