using blazchat.Domain.Entities;

namespace blazchat.Infra.Data.Interfaces
{
    public interface IChatUsersRepository : IRepository<ChatUser>
    {
        Task<bool> ValidateChatAsync(Guid chatId, Guid userId);
        Task<Guid> GetGuessUserByChatId(Guid chatId, Guid currentUser);
    }
}
