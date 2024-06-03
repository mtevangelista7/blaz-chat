using blazchat.Application.Interfaces.Services;
using blazchat.Domain.Entities;
using blazchat.Infra.Data.Interfaces;

namespace blazchat.Application.Services;

public class MessageService(IMessageRepository messageRepository) : IMessageService
{
    public async Task AddMessageAsync(Message message)
    {
        await messageRepository.AddMessageAsync(message);
    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await messageRepository.GetMessagesByChatIdAsync(chatId);
    }
}