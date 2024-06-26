﻿using blazchat.Infra.Interfaces;
using blazchat.Services.Interfaces;

namespace blazchat.Endpoints.Messages;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        app.MapGet("/api/messages/{chatId:guid}", async (Guid chatId, IMessageService messageService) =>
        {
            var messages = await messageService.GetMessagesByChatIdAsync(chatId);
            return Results.Ok(messages);
        });
    }
}