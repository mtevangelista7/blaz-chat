using blazchat.Domain.Entities;

namespace blazchat.Application.Interfaces.Services;

public interface IMessageService
{
    public Task AddMessageAsync(Message message);

    public Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
}