using blazchat.Entities;
using blazchat.Infra.Interfaces;
using blazchat.Services.Interfaces;

namespace blazchat.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task AddMessageAsync(Message message)
    {
        await _messageRepository.AddMessageAsync(message);
    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await _messageRepository.GetMessagesByChatIdAsync(chatId);
    }
}