using blazchat.Entities;

namespace blazchat.Services.Interfaces;

public interface IMessageService
{
    public Task AddMessageAsync(Message message);

    public Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
}