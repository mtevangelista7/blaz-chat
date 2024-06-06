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
                    List<Domain.Entities.Chat> activeChats = await chatService.GetActiveChats(userId);

                    List<ChatDto> result = new List<ChatDto>();
                    foreach (var chat in activeChats)
                    {
                        var chatDto = new ChatDto(
                            Id: chat.Id,
                            ChatUsers: chat.ChatUsers.Select(x => new ChatUserDto
                            {
                                UserId = x.UserId,
                                ChatId = x.ChatId,
                                User = new CreateUserDto(Username: x.User.Username, Password: null)
                            }).ToList());

                        result.Add(chatDto);
                    }

                    return Results.Ok(result);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            });
        }
    }
}