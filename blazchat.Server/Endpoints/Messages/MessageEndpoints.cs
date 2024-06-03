using blazchat.Application.Interfaces.Services;

namespace blazchat.Server.Endpoints.Messages;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        app.MapGet("/api/messages/{chatId:guid}", async (Guid chatId, IMessageService messageService) =>
        {
            try
            {
                var messages = await messageService.GetMessagesByChatIdAsync(chatId);
                return Results.Ok(messages);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }).RequireAuthorization();
    }
}