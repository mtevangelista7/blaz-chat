using blazchat.Application.DTOs;
using blazchat.Client.Entities;
using Refit;

namespace blazchat.Client.RefitInterfaceApi;

public interface IMessageEndpoints
{
    [Get("/api/messages/{chatId}")]
    Task<List<MessageDto>> GetMessages(Guid chatId);
}