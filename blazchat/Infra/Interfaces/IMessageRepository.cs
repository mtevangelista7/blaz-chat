using blazchat.Entities;

namespace blazchat.Infra.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
    Task AddMessageAsync(Message message);
}