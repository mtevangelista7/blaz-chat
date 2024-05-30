using blazchat.Entities;

namespace blazchat.Services.Interfaces;

public interface IChatService
{
    public Task<Guid> CreateNewChat(Guid user1Id, Guid user2Id);

    public Task<bool> ValidateChat(Guid chatId, Guid userId);
    Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser);

    public Task<Chat> GetChat(Guid chatId);

    public Task<List<Chat>> GetActiveChats(Guid userId);
}