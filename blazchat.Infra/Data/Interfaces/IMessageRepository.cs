namespace blazchat.Infra.Data.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
    Task AddMessageAsync(Message message);
}