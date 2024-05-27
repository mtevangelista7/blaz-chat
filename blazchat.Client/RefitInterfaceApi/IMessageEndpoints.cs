using blazchat.Client.Dtos;
using Refit;

namespace blazchat.Client.RefitInterfaceApi;

public interface IMessageEndpoints
{
    [Get("/api/messages/{chatId:guid}")]
    Task<List<MessageDto>> GetMessages(Guid chatId);
}