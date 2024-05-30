using blazchat.Entities;

namespace blazchat.Infra.Interfaces
{
    public interface IChatUsersRepository
    {
        Task<bool> ValidateChatAsync(Guid chatId, Guid userId);
        Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser);
    }
}
