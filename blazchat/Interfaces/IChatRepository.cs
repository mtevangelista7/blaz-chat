using blazchat.Entities;

namespace blazchat.Interfaces;

public interface IChatRepository
{
    Task<Chat> GetChatAsync(Guid chatId);
    Task<Chat> CreateChatAsync(Guid user1Id, Guid user2Id);
    Task<List<Chat>> GetActiveChatsAsync(Guid userId);
}